"use strict";
import React from 'react';
import { Link } from 'react-router'
import { connect } from 'react-redux'
import { loginAction, loginReducer } from '../../reducers/auth/login.js'

class Login extends React.Component {
    static contextTypes = {
        router: React.PropTypes.object.isRequired
    };
    state = {
        loginName: '',
        password: ''
    };
    constructor(props) {
        super(props);
    }
    render() {
        const { loginStore, dispatch } = this.props
        return (
            <div>
                <h1>Login : {loginStore ? '已登陆' : '未登录'}</h1>
                <p>LoginName:<input name="loginName" placeholder="请输入用户名" onChange={e=>this.setState({loginName:e.target.value})} /></p>    
                <p>Password:<input name="password" placeholder="请输入密码" onChange={e=>this.setState({password:e.target.value})} /></p>    
                <p>
                    <button onClick={e=>dispatch(loginAction.login(this.state.loginName,this.state.password))}>登陆</button>
                    <button onClick={e=>this.context.router.goBack()}>返回</button>
                </p>
            </div>
        );
    }
}

export default connect(state => ({loginStore: state.loginReducer}))(Login);