<#
azure functions change app service plan -> https://github.com/Azure/Azure-Functions/issues/155 
#>

# Execute following command once before running any azure provisioning script to log you into your azure subscription(s)
#Connect-AzureRmAccount

# Provide the subscription Id
$subscriptionId = '1336717a-463c-4c74-b90f-a357edd79989'

# Set the context to the subscription Id where Managed Disk will be created
Select-AzureRmSubscription -SubscriptionId $SubscriptionId

# Provide the name of your resource group, function app and pay-as-you-go app service plan
$resourceGroupName = 'EmUamRgn'
$functionAppName = 'emuamfuncapp'
$paygAppServicePlan = 'emappsvcplanpaygfuncapp'

Set-AzureRmWebApp -ResourceGroupName $resourceGroupName -Name $functionAppName -AppServicePlan $paygAppServicePlan
