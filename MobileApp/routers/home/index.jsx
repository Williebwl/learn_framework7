"use strict";
import React from 'react';
import { Link } from 'react-router'
import { connect } from 'react-redux'
import { checkLogin } from '../../app/index.js'
import { userAction, userReducer } from '../../reducers/system/user.js'

class Index extends React.Component {
    render() {
        const { userStore, dispatch } = this.props
        return (
            <section>
                <h1>Index Page</h1>
                <ul>
                    <li><a onClick={e=>dispatch(userAction.post({Title: '测试'+Math.random()}))}>新增</a></li>
                    <li><a onClick={e=>checkLogin() || dispatch(userAction.get())}>读取</a></li>
                    <li><Link to="/auth/login">登陆</Link></li>
                </ul>
                <ul>
                    {userStore.map((item, index) => <li key={item.ID}>
                        <a href={item.ArticleUrl}>{item.Title}</a>
                        <a onClick={()=>dispatch(userAction.put(item.ID, {Title: '测试'+Math.random()}))}>[更新]</a>
                        <a onClick={()=>dispatch(userAction.delete(item.ID))}>[删除]</a>
                    </li>)}
                </ul>
            </section>
        );
    }
}

export default connect(state => ({userStore: state.userReducer}))(Index);