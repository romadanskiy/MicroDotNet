import React, {useState} from 'react';

type TokenResponse = {
  access_token: string
};

const App = function () {
  const [token, setToken] = useState('');

  const onClick = async () => {
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

    const res = await fetch('http://localhost:5000/connect/token', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: formBodyStr
    });
    
    const data: TokenResponse = await res.json();
    setToken(data.access_token);
  };
  return <div className="App">
    <button onClick={onClick}>Test Token</button>
    <div>{token}</div>
  </div>;
}
export default App;