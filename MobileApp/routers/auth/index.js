"use strict";
import login from './login.jsx'

export const authRouter = { 
    path: 'auth',
    childRoutes: [
        { path: 'login', component: login }
    ]
};