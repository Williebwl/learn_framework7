"use strict";
import { homeRouter } from './home/index.js'
import { authRouter } from './auth/index.js';
import { systemRouter } from './system/index.js';

export const routers = [{
    path: '/',
    indexRoute: homeRouter,
    childRoutes: [
        authRouter, systemRouter
    ]
}];