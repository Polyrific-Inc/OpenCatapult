# Storage Provider

Storage Providers can be used in [Publish task](../user-guides/job-definitions.md#publishartifact). The main role of this provider is to upload or download a build artifact into a storage location.

There's currently no built-in provider for this since it is not needed in the current default CI/CD workflow. We will add this in the future once we support another complex workflow, like the usage of external devops tools such as azure dev ops.