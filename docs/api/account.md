# Account

These endpoints provide methods to add, edit, and delete account related data

## Register

Register a new application user.

|||
| --- | --- |
| Method | `POST` |
| Uri | `/Account/Register` |
| Authorization | `Anonymous` |

## Confirm Email

Confirm the registered email using the confirmation token.

|||
| --- | --- |
| Method | `GET` |
| Uri | `/Account/{userId}/Confirm?token={token}` |
| Authorization | `Anonymous` |

## Get Users

Get the list of application users. Optionally filter it by status (`active` | `suspended`)

|||
| --- | --- |
| Method | `GET` |
| Uri | `/Account?status={status}` |
| Authorization | `UserRoleAdminAccess` |

## Get User By Id

Get a user by user Id. Only administrator would be able to call this endpoint for other user Id. The rest would only be accepted if the supplied `userId` is of the current user.

|||
| --- | --- |
| Method | `GET` |
| Uri | `/Account/{userId}` |
| Authorization | `Authenticated` |

## Get User By Email

Get a user by its email. Only administrator would be able to call this endpoint for other user email. The rest would only be accepted if the supplied `email` is of the current user.

|||
| --- | --- |
| Method | `GET` |
| Uri | `/Account/email/{email}` |
| Authorization | `Authenticated` |

## Get Current user

Get the current user data.

|||
| --- | --- |
| Method | `GET` |
| Uri | `/Account/CurrentUser` |
| Authorization | `Authenticated` |

## Update User Profile

Update user profile (i.e. First Name and Last Name). Only administrator would be able to call this endpoint for other user Id. The rest would only be accepted if the supplied `userId` is of the current user.

|||
| --- | --- |
| Method | `PUT` |
| Uri | `/Account/{userId}` |
| Authorization | `Authenticated` |

## Delete User

Delete a user.

|||
| --- | --- |
| Method | `DELETE` |
| Uri | `/Account/{userId}` |
| Authorization | `UserRoleAdminAccess` |

## Suspend user

Suspend a user to prevent her to login into the application.

|||
| --- | --- |
| Method | `POST` |
| Uri | `/Account/{userId}/suspend` |
| Authorization | `UserRoleAdminAccess` |

## Reactivate user

Reactivate a suspended user to allow her to regain access to the application.

|||
| --- | --- |
| Method | `POST` |
| Uri | `/Account/{userId}/activate` |
| Authorization | `UserRoleAdminAccess` |

## Update password

Update a user password. Only administrator would be able to call this endpoint for other user Id. The rest would only be accepted if the supplied `userId` is of the current user.

|||
| --- | --- |
| Method | `POST` |
| Uri | `/Account/{userId}/password` |
| Authorization | `Authenticated` |

## Request reset password

Request for a reset password token to be sent to user email address.

|||
| --- | --- |
| Method | `GET` |
| Uri | `/Account/email/{email}/resetpassword` |
| Authorization | `Anonymous` |

## Reset password

Update user's password by specifying the request password token.

|||
| --- | --- |
| Method | `POST` |
| Uri | `/Account/email/{email}/resetpassword` |
| Authorization | `Anonymous` |

## Set User Role

Set role of the user. Available roles are: `Administrator`, `Basic`, or `Guest`

|||
| --- | --- |
| Method | `POST` |
| Uri | `/Account/{userId}/role` |
| Authorization | `UserRoleAdminAccess` |
