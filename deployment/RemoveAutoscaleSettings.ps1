<#
azure app service delete autoscale settings -> https://blogs.msdn.microsoft.com/benjaminperkins/2017/08/10/remove-auto-scaling-from-an-azure-app-service-using-azure-powershell/
#>

# Execute following command once before running any azure provisioning script to log you into your azure subscription(s)
#Connect-AzureRmAccount

# Provide the subscription Id
$subscriptionId = '1336717a-463c-4c74-b90f-a357edd79989'

# Set the context to the subscription Id where Managed Disk will be created
Select-AzureRmSubscription -SubscriptionId $SubscriptionId

# Provide the name of your resource group
$resourceGroupName = 'EmUamRgn'
Get-AzureRmAutoscaleSetting -ResourceGroup $resourceGroupName

$autoscaleSettingNameRoot = 'myautoscalesetup'
Remove-AzureRmAutoscaleSetting -ResourceGroup $resourceGroupName -Name ($autoscaleSettingNameRoot + 'funcapp')
Remove-AzureRmAutoscaleSetting -ResourceGroup $resourceGroupName -Name ($autoscaleSettingNameRoot + 'webapp')
Remove-AzureRmAutoscaleSetting -ResourceGroup $resourceGroupName -Name ($autoscaleSettingNameRoot + 'vmiisapp')

Get-AzureRmAutoscaleSetting -ResourceGroup $resourceGroupName

