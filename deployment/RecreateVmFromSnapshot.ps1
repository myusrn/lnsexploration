<# 
powershell azure vm disk create snapshot -> 
https://docs.microsoft.com/en-us/azure/virtual-machines/windows/snapshot-copy-managed-disk -> 
https://docs.microsoft.com/en-us/azure/virtual-machines/scripts/virtual-machines-windows-powershell-sample-create-vm-from-snapshot?toc=%2fpowershell%2fmodule%2ftoc.json 
#>

# Execute following command once before running any azure provisioning script to log you into your azure subscription(s)
#Connect-AzureRmAccount

# Provide the subscription Id
$subscriptionId = '1336717a-463c-4c74-b90f-a357edd79989'

# Provide the name of your resource group
$resourceGroupName = 'EmUamRgn'

# Provide the name of the snapshot that will be used to create OS disk
$snapshotName = 'emuamvmiisapp_OsDisk_1_a9b2f2875d724bb69c65d27eb76389a3_PreSysprepOobeGeneralize'
#$snapshotName = 'emuamvmiisapp_OsDisk_1_PreSysprepOobeGeneralize'

# Provide the name of the OS disk that will be created using the snapshot
$osDiskName = 'emuamvmiisapp_OsDisk_1_a9b2f2875d724bb69c65d27eb76389a3'

# Provide the name of the virtual machine
$virtualMachineName = 'emuamvmiisapp'

# Provide the size of the virtual machine, get all the vm sizes in a region using: Get-AzureRmVMSize -Location westus2
$virtualMachineSize = 'Standard_DS1_v2'

# Set the context to the subscription Id where Managed Disk will be created
Select-AzureRmSubscription -SubscriptionId $SubscriptionId

# Create or Get the virtual machine disk from snapshot
#$snapshot = Get-AzureRmSnapshot -ResourceGroupName $resourceGroupName -SnapshotName $snapshotName
#$diskConfig = New-AzureRmDiskConfig -Location $snapshot.Location -SourceResourceId $snapshot.Id -CreateOption Copy
#$disk = New-AzureRmDisk -Disk $diskConfig -ResourceGroupName $resourceGroupName -DiskName $osDiskName
$disk = Get-AzureRmDisk -Disk $diskConfig -ResourceGroupName $resourceGroupName -DiskName $osDiskName

# Initialize virtual machine configuration
$virtualMachine = New-AzureRmVMConfig -VMName $virtualMachineName -VMSize $virtualMachineSize

# Use the Managed Disk Resource Id to attach it to the virtual machine. Please change the OS type to linux if OS disk has linux OS
$virtualMachine = Set-AzureRmVMOSDisk -VM $virtualMachine -ManagedDiskId $disk.Id -CreateOption Attach -Windows

# Get the virtual network where virtual machine will be hosted
$virtualNetworkName = 'EmUamRgn-vnet'
$vnet = Get-AzureRmVirtualNetwork -ResourceGroupName $resourceGroupName -Name $virtualNetworkName 

# Create or Get a public IP for the VM
#$publicIp = New-AzureRmPublicIpAddress -ResourceGroupName $resourceGroupName -Name ($VirtualMachineName.ToLower()+'_ip') -Location $snapshot.Location -AllocationMethod Dynamic
#$publicIpName = 'emuamvmiisapp-ip'; $publicIp = New-AzureRmPublicIpAddress -ResourceGroupName $resourceGroupName -Name $publicIpName -Location $snapshot.Location -AllocationMethod Dynamic
$publicIpName = 'emuamvmiisapp-ip'; $publicIp = Get-AzureRmPublicIpAddress -ResourceGroupName $resourceGroupName -Name $publicIpName 

# Get the network security group that will control virtual machine exposed ports
$nsgName = 'emuamvmiisapp-nsg'; $nsg = Get-AzureRmNetworkSecurityGroup -ResourceGroupName $resourceGroupName -Name $nsgName 

# Create or Get NIC in the first subnet of the virtual network
#$nic = New-AzureRmNetworkInterface -Name ($VirtualMachineName.ToLower()+'_nic') -ResourceGroupName $resourceGroupName -Location $snapshot.Location -SubnetId $vnet.Subnets[0].Id -PublicIpAddressId $publicIp.Id
#$nicName = 'emuamvmiisapp205'; $nic = New-AzureRmNetworkInterface -Name $nicName -ResourceGroupName $resourceGroupName -Location $snapshot.Location -SubnetId $vnet.Subnets[0].Id -PublicIpAddressId $publicIp.Id -NetworkSecurityGroupId $nsg.Id
$nicName = 'emuamvmiisapp205'; $nic = Get-AzureRmNetworkInterface -Name $nicName -ResourceGroupName $resourceGroupName

# Assign nic to virtual machine
$virtualMachine = Add-AzureRmVMNetworkInterface -VM $virtualMachine -Id $nic.Id

# Create or disable virtual machien os disk boot disagnostics which if enabled requires a storage account assignment
#$storageAccountName = 'emstrgacct'; Set-AzureRmVMBootDiagnostics -VM $virtualMachine -Enable -ResourceGroupName $resourceGroupName -StorageAccountName $storageAccountName
Set-AzureRmVMBootDiagnostics -VM $virtualMachine -Disable

# Create the virtual machine with Managed Disk
New-AzureRmVM -VM $virtualMachine -ResourceGroupName $resourceGroupName -Location $snapshot.Location

# Get the virtual machine with Managed Disk
#$vm = Get-AzureRmVM -ResourceGroupName $resourceGroupName -Name $virtualMachineName

