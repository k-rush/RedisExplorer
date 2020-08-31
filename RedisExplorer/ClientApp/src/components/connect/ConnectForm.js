import React, { Component } from 'react';

export default class ConnectForm extends Component {
    constructor(props) {
        super(props);
        this.state = { host: '', password: '' };

        this.handleSubmit = this.handleSubmit.bind(this);
        this.setHost = this.setHost.bind(this);
        this.setPassword = this.setPassword.bind(this);
    }

    handleSubmit(event) {
        console.log(JSON.stringify(this.state));
        fetch('connect', {
            method: 'POST',
            headers: {
                'Accept': 'application/json, text/plain',
                'Content-Type': 'application/json;charset=UTF-8'
            },
            body: JSON.stringify(this.state)
        });
    }

    setHost(event) {
        this.setState({ host: event.target.value });
    }

    setPassword(event) {
        this.setState({ password: event.target.value });
    }

    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <label>
                    Host:
                    <input type="text" value={this.state.host} onChange={this.setHost} />
                </label>
                <label>
                    Password:
                    <input type="text" value={this.state.password} onChange={this.setPassword} />
                </label>
                <input type="submit" value="Submit" />
            </form>
        );
    }
}