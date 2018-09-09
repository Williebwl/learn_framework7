"use strict";
export function NetworkError(url, message) {
    this.name = 'NetworkError';
    this.url = url;
    this.message = message;
    this.stack = (new Error()).stack;
}
NetworkError.prototype = new Error;