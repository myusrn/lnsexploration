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

# 1. Prepare customized vm for generalization
#Stop-AzureRmVM -ResourceGroupName $resourceGroupName -Name $customizedVmName -Force
#Set-AzureRmVM -ResourceGroupName $resourceGroupName -Name $customizedVmName -Generalized 

# 2. Get VM object
#$customizedVm = Get-AzureRmVM -ResourceGroupName $resourceGroupName -Name $customizedVmName

# 3. Create the VM image configuration based on the source VM "powershell new-azurermimageconfig -sourcevirtualmachineid new-azurermimage"
#$imageConfig = New-AzureRmImageConfig -Location $customizedVm.Location -SourceVirtualMachineId $customizedVm.Id 

# 4a. Create or Get the custom VM image
$customizedVmImageName = 'emuamvmssapp-image'
#$image = New-AzureRmImage -ResourceGroupName $resourceGroupName -Image $imageConfig -ImageName $customizedVmImageName 

# 4b. Create or Get the custom VM image
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

New-AzureRmVmss -ResourceGroupName $resourceGroupName -VMScaleSetName $vmssName -ImageName $customizedVmImageName -Credential $vmCredential -VirtualNetworkName $virtualNetworkName -SubnetName $vnetSubnetName -PublicIpAddressName $publicIpName -LoadBalancerName $lbName -Location 'WestUS2'
#New-AzureRmVmss -ResourceGroupName $resourceGroupName -VMScaleSetName $vmssName `
#  -ImageName $customizedVmImageName -Credential $vmCredential `
#  -VirtualNetworkName $virtualNetworkName -SubnetName $vnetSubnetName -PublicIpAddressName $publicIpName `
#  -LoadBalancerName $lbName ` # for application gateway frontend see application-gateway/tutorial-create-vmss-powershell reference at top of script
#  -Location 'WestUS2' `
#  -UpgradePolicyMode 'Automatic' ` # vs 'Manual' or 'Rolling'
#  -AllocationMethod 'Dynamic' # vs 'Static' ip addresses  
# if you don't provide vmss image administrator username and password credentials then expect a popup dialog to prompt you for them
# the load balancer has rule for http tcp/80 enabled by default and nat settings for rdp 3389 and winrm 5985 by default

#Get-AzureRmPublicIpAddress -ResourceGroupName $resourceGroupName -Name $publicIpName | Select IpAddress
#Get-AzureRmPublicIpAddress -ResourceGroupName $resourceGroupName -Name $publicIpName | Select -ExpandProperty DnsSettings | Select Fqdn
# https://docs.microsoft.com/en-us/powershell/module/azurerm.network/set-azurermpublicipaddress
# emuamvmssapp-80dfae.westus2.cloudapp.azure.com
$publicIp = Get-AzureRmPublicIpAddress -ResourceGroupName $resourceGroupName -Name $publicIpName
write-host 'ipAddress =' $publicIp.IpAddress 'and fqdn =' $publicIp.DnsSettings.Fqdn
$publicIp.DnsSettings.DomainNameLabel = $vmssName; Set-AzureRmPublicIpAddress -PublicIpAddress $publicIp
$publicIp = Get-AzureRmPublicIpAddress -ResourceGroupName $resourceGroupName -Name $publicIpName
write-host 'ipAddress =' $publicIp.IpAddress 'and fqdn =' $publicIp.DnsSettings.Fqdn

$publicSettings = @{ "fileUris" = (,"https://raw.githubusercontent.com/Azure/azure-docs-powershell-samples/master/application-gateway/iis/appgatewayurl.ps1"); 
  "commandToExecute" = "powershell -ExecutionPolicy Unrestricted -File appgatewayurl.ps1" }
$vmss = Get-AzureRmVmss -ResourceGroupName $resourceGroupName -VMScaleSetName $vmssName `
Add-AzureRmVmssExtension -VirtualMachineScaleSet $vmss -Name "customScript" -Publisher "Microsoft.Compute" `
  -Type "CustomScriptExtension" -TypeHandlerVersion 1.8 -Setting $publicSettings 
Update-AzureRmVmss -ResourceGroupName $resourceGroupName -Name $vmssName -VirtualMachineScaleSet $vmss
