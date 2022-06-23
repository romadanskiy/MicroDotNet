import { useState } from "react";

type TokenResponse = {
    access_token: string
  };

export interface Response {
    token: string
    user: {
        Email: string
        Password: string
    }
}

export async function signIn(Email: string, Password: string): Promise<Response>{

          const request:any = {
            grant_type: "password",
            username: Email,
            password: Password
          };
          console.log("apiFunction: ", request)
      
          const formBody = new FormData();
          for (let property in request) {
            formBody.append(property, request[property]);
          } 
          console.log("formBody: ", formBody)

          const res = await fetch('http://localhost:5000/connect/token', {
            method: 'POST',
            headers: {
              'Content-Type': 'multipart/form-data',
              'Accept': '*/*'
              
            },
            body: formBody
          });
          console.log("res: ", res)
          const data: TokenResponse = await res.json();
          console.log("data: ", data)


    //const [token, setToken] = useState('');
      return ({
        token: data.access_token,
        user: {
            Email: Email,
            Password: Password
        },
    });
    
}

/*


{
                token: data.access_token,
                user: {
                    Email: Email,
                    Password: Password
                },
            }

  return new Promise((resolve) => {
        setTimeout(() => {
            resolve({
                token: '45h46hhlth5ih6lhy5hl45h6lj7hlh4lhl56h54l',
                user: {
                    name: 'diego',
                    email:'deego@mail.ru',
                },
            });
        }, 2000)
    });
    */