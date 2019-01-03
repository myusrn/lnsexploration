<#
powershell create virtual machine scale set using application gateway -> 
https://docs.microsoft.com/en-us/azure/application-gateway/tutorial-create-vmss-powershell

powershell create virtual machine scale set from custom image -> 
https://docs.microsoft.com/en-us/azure/virtual-machine-scale-sets/tutorial-use-custom-image-powershell
https://docs.microsoft.com/en-us/powershell/module/azurerm.compute/new-azurermimageconfig
https://docs.microsoft.com/en-us/powershell/module/azurerm.compute/new-azurermvmss
#>

# Execute following command once before running any azure provisioning script to log you into your azure subscription(s)
#Connect-AzureRmAccount

# Provide the subscription Id
$subscriptionId = '1336717a-463c-4c74-b90f-a357edd79989'

# Provide the name of your resource group
$resourceGroupName = 'EmUamRgn'

# Provide the name of your customized vm to create generliazed image from
$customizedVmName = 'emuamvmiisapp'

# Set the context to the subscription Id where Managed Disk will be created
Select-AzureRmSubscription -SubscriptionId $SubscriptionId

# *** before proceeding publish your latest application bits, stop vm and create snapshot of customized vm OsDisk now if you want ability to get back to origin setup later 
# *** do this by visiting OsDisk resource and selecting create snapshot and assign name that matches the currently uncommented value in RecreateVmFromSnapshot.ps1 script
# *** note that location of resource group and vm OsDisk snapshot controls what vm restore and vm scale set image scenarios you can use snapshot for 

# *** before proceeding execute sysprep generalization on customized vm and shutdown
# *** do this by establishing connection to vm and executing %windir%\system32\sysprep\sysprep.exe /oobe /generalize /shutdown
# *** once that is done you can proceed with the following script commands

# Prepare customized vm for generalization
Stop-AzureRmVM -ResourceGroupName $resourceGroupName -Name $customizedVmName -Force
Set-AzureRmVM -ResourceGroupName $resourceGroupName -Name $customizedVmName -Generalized 

# Get VM object
$customizedVm = Get-AzureRmVM -ResourceGroupName $resourceGroupName -Name $customizedVmName

# Create the VM image configuration based on the source VM "powershell new-azurermimageconfig -sourcevirtualmachineid new-azurermimage"
$imageConfig = New-AzureRmImageConfig -Location $customizedVm.Location -SourceVirtualMachineId $customizedVm.Id 

# Create or Get the custom VM image
$customizedVmImageName = 'emuamvmssapp-image'
#$image = New-AzureRmImage -ResourceGroupName $resourceGroupName -Image $imageConfig -ImageName $customizedVmImageName 
$image = Get-AzureRmImage -ResourceGroupName $resourceGroupName -ImageName $customizedVmImageName 

# Create virtual machine scale set from custom VM image
$vmssName = 'emuamvmssapp'
$virtualNetworkName = 'EmUamRgn-vnet'
$vnet = Get-AzureRmVirtualNetwork -ResourceGroupName $resourceGroupName -Name $virtualNetworkName 
$vnetSubnetName = $vnet.Subnets[0].Name # in case of applicationGateway not allowed to use .Subnets[0].Name = default
$publicIpName = 'emuamvmssapp-ip'
#$nsgName = 'emuamvmssapp-nsg';
#$nsg = Get-AzureRmNetworkSecurityGroup -ResourceGroupName $resourceGroupName -Name $nsgName 

$vmPassword = ConvertTo-SecureString 'P@ssw0rd1234' -AsPlainText -Force
$vmCredential = New-Object System.Management.Automation.PSCredential('vmLogon', $vmPassword)
$lbName = $vmssName + '-lb'

New-AzureRmVmss -ResourceGroupName $resourceGroupName -VMScaleSetName $vmssName `
  -ImageName $customizedVmImageName -Credential $vmCredential `
  -VirtualNetworkName $virtualNetworkName -SubnetName $vnetSubnetName -PublicIpAddressName $publicIpName ` # first not recognized and all defaulted to $vmssName  
  -LoadBalancerName $lbName ` # no -ApplicationGatewayName $agName option and if not specified creates lb with $lbName = $vmssName
  -Location 'WestUS2' `
  #-UpgradePolicyMode 'Automatic' ` # vs 'Manual' or 'Rolling'
  #-AllocationMethod 'Dynamic' ` # vs 'Static' ip addresses  
# expect command to prompt you for the vmss image administrator username and password credentials even thougy you are providing them as a parameter
# load balancer has http 80, rdp 3389, winrm 5985 port access enabled by default

Get-AzureRmPublicIpAddress -ResourceGroupName $resourceGroupName -Name $publicIpName | Select IpAddress # | Select -ExpandProperty DnsSettings | Select Fqdn
# http://52.183.99.131/index.html
Get-AzureRmPublicIpAddress -ResourceGroupName $resourceGroupName -Name $publicIpName | Select -ExpandProperty DnsSettings | Select Fqdn
# http://emuamvmssapp-1381c1.WestUS2.cloudapp.azure.com/index.html

$publicSettings = @{ "fileUris" = (,"https://raw.githubusercontent.com/Azure/azure-docs-powershell-samples/master/application-gateway/iis/appgatewayurl.ps1"); 
  "commandToExecute" = "powershell -ExecutionPolicy Unrestricted -File appgatewayurl.ps1" }
$vmss = Get-AzureRmVmss -ResourceGroupName $resourceGroupName -VMScaleSetName $vmssName `
Add-AzureRmVmssExtension -VirtualMachineScaleSet $vmss -Name "customScript" -Publisher "Microsoft.Compute" `
  -Type "CustomScriptExtension" -TypeHandlerVersion 1.8 -Setting $publicSettings 
Update-AzureRmVmss -ResourceGroupName $resourceGroupName -Name $vmssName -VirtualMachineScaleSet $vmss
