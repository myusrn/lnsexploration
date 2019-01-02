# Provide the subscription Id
$subscriptionId = '1336717a-463c-4c74-b90f-a357edd79989'

# Provide the name of your resource group
$resourceGroupName = 'EmUamRgn'

# Provide the name of the snapshot that will be used to create OS disk
$snapshotName = 'PreSysprepGeneralize'

# Provide the name of the OS disk that will be created using the snapshot
$osDiskName = 'emuamvmiisapp_OsDisk_1_a9b2f2875d724bb69c65d27eb76389a3'

# Provide the name of the virtual machine
$virtualMachineName = 'emuamvmiisapp'

# Provide the size of the virtual machine, get all the vm sizes in a region using: Get-AzureRmVMSize -Location westus2
$virtualMachineSize = 'Standard_DS1_v2'

# Execute following command before running script to login to subscription(s)
#Connect-AzureRmAccount

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
$vnet = Get-AzureRmVirtualNetwork -Name $virtualNetworkName -ResourceGroupName $resourceGroupName

# Create or Get a public IP for the VM
#$publicIp = New-AzureRmPublicIpAddress -Name ($VirtualMachineName.ToLower()+'_ip') -ResourceGroupName $resourceGroupName -Location $snapshot.Location -AllocationMethod Dynamic
#$publicIpName = 'emuamvmiisapp-ip'; $publicIp = New-AzureRmPublicIpAddress -Name $publicIpName -ResourceGroupName $resourceGroupName -Location $snapshot.Location -AllocationMethod Dynamic
$publicIpName = 'emuamvmiisapp-ip'; $publicIp = Gew-AzureRmPublicIpAddress -Name $publicIpName -ResourceGroupName $resourceGroupName

# Get the network security group that will control virtual machine exposed ports
$nsg = Get-AzureRmNetworkSecurityGroup -Name $nsgName -ResourceGroupName $resourceGroupName

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

