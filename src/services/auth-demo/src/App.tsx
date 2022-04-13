import React, {useState} from 'react';

type TokenResponse = {
  access_token: string
};

const App = function () {
  const [token, setToken] = useState('');

  const getToken = async () => {
    const request:any = {
      grant_type: "password",
      username: "Admin",
      password: "qWe!123"
    };

    const formBody = [];
    for (let property in request) {
      const encodedKey = encodeURIComponent(property);
      const encodedValue = encodeURIComponent(request[property]);
      formBody.push(encodedKey + "=" + encodedValue);
    }
    
    const formBodyStr = formBody.join("&");

    const res = await fetch('/connect/token', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: formBodyStr
    });
    
    const data: TokenResponse = await res.json();
    setToken(data.access_token);
  };

  const sendAuthorize = () => {
    fetch('/example',  {
      method: 'GET',
      headers: {
        "Accept": "application/json",
        "Authorization": `Bearer ${token}`
      }
    });
  }

  const sendUnauthorized = () => {
    fetch('/example', {
      method: 'GET',
      headers: {},
    });    
  }

  return <div className="App">
    <button onClick={getToken}>Test Token</button>
    <div>{token}</div>
    <button disabled={token === ""} onClick={sendAuthorize}>Send authorized</button>
    <button onClick={sendUnauthorized}>Send unauthorized</button>
  </div>;
}
export default App;