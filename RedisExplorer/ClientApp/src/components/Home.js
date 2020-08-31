import React, { Component } from 'react';
import ConnectForm from './connect/ConnectForm';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <ConnectForm />
        );
    }
}
