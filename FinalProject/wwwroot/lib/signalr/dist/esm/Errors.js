// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
export class HttpError extends Error {
    constructor(errorMessage, statusCode) {
        super(errorMessage);
        this.statusCode = statusCode;
    }
}
export class TimeoutError extends Error {
    constructor(errorMessage = "A timeout occurred.") {
        super(errorMessage);
    }
}
//# sourceMappingURL=Errors.js.map