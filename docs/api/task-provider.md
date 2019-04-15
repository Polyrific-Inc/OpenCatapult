# Task Provider

These endpoints provide methods to register, remove, and get info of task providers within the catapult instalation

## Get Task Provider List
Method: `GET`

Uri: `/task-provider`

Authorization: `UserRoleAdminAccess`

Get list of task providers installed within the catapult.

## Register Task Provider
Method: `POST`

Uri: `/task-provider`

Authorization: `UserRoleAdminAccess`

Register a task provider into the catapult by uploading the .yml file that describe the task provider metadata.

## Get Task Providers by type
Method: `GET`

Uri: `/task-provider/type/{taskProviderType}`

Authorization: `UserRoleAdminAccess`

Get list of task providers, filtered by type (all | BuildProvider | DatabaseProvider | GeneratorProvider | HostingProvider | RepositoryProvider | StorageProvider | TestProvider)

## Get Task Provider By Id
Method: `GET`

Uri: `/task-provider/{taskProviderId}`

Authorization: `UserRoleBasicAccess`

Get a task provider by its Id

## Remove Task Provider
Method: `DELETE`

Uri: `/task-provider/{taskProviderId}`

Authorization: `UserRoleAdminAccess`

Remove a task provider from catapult

## Get Task Provider By Name
Method: `GET`

Uri: `/task-provider/name/{taskProviderName}`

Authorization: `UserRoleBasicAccess`

Get a provider by its Name

## Get Task Provider's Configs
Method: `GET`

Uri: `/task-provider/name/{taskProviderName}/config`

Authorization: `UserRoleBasicAccess`

Get list of additional configs of a task provider