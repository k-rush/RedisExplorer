import React, { Component } from 'react';

export class KeysList extends Component {
    static displayName = KeysList.name;

  constructor(props) {
    super(props);
    this.state = { keys: [], loading: true };
  }

  componentDidMount() {
      this.getKeys();
  }

  static renderKeysTable(keys) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Key</th>
            <th>Type</th>
          </tr>
        </thead>
        <tbody>
          {keys.map(key =>
            <tr key={key.key}>
              <td>{key.key}</td>
              <td>{key.type}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
        : KeysList.renderKeysTable(this.state.keys);

    return (
      <div>
        <h1 id="tabelLabel" >Redis keys</h1>
        {contents}
      </div>
    );
  }

  async getKeys() {
    const response = await fetch('keys');
    const data = await response.json();
    this.setState({ keys: data, loading: false });
  }
}
