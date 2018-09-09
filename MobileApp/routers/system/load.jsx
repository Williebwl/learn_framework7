"use strict";
import React from 'react';
import { browserHistory } from 'react-router'

export default class Home extends React.Component {
    static contextTypes = {
        router: React.PropTypes.object.isRequired
    };
    componentDidMount() {
        this.context.router.goBack();
    }
    render() {
        return (<h1>Load</h1>);
    }
}