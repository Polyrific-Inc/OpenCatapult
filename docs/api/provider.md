# Provider

These endpoints provide methods to register, remove, and get info of task providers within the catapult instalation

## Get Provider List
Method: `GET`

Uri: `/Provider`

Authorization: `UserRoleAdminAccess`

Get list of task providers installed within the catapult.

## Register Provider
Method: `POST`

Uri: `/Provider`

Authorization: `UserRoleAdminAccess`

Register a task provider into the catapult by uploading the .yml file that describe the task provider metadata.

## Get Providers by Provider Type
Method: `GET`

Uri: `/provider/type/{providerType}`

Authorization: `UserRoleAdminAccess`

Get list of task providers, filtered by type (all | BuildProvider | DatabaseProvider | GeneratorProvider | HostingProvider | RepositoryProvider | StorageProvider | TestProvider)

## Get Provider By Id
Method: `GET`

Uri: `/Provider/{providerId}`

Authorization: `UserRoleBasicAccess`

Get a task provider by its Id

## Remove Provider
Method: `DELETE`

Uri: `/Provider/{providerId}`

Authorization: `UserRoleAdminAccess`

Remove a task provider from catapult

## Get Provider By Name
Method: `GET`

Uri: `/Provider/name/{providerName}`

Authorization: `UserRoleBasicAccess`

Get a provider by its Name

## Get Provider's Configs
Method: `GET`

Uri: `/provider/name/{providerName}/config`

Authorization: `UserRoleBasicAccess`

Get list of additional configs of a task provider