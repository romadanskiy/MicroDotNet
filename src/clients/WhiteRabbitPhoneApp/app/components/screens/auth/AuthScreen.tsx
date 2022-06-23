import React, { FC, useState, useContext } from "react";
import { View, Text, StyleSheet, Button, Pressable } from "react-native";
import Field from "../ui/Field";
import Loader from "../ui/Loader";
//import { signIn } from "./authOIDC";
import  AuthContext  from './contexts/auth';


interface IData {
    FirstName: string
    LastName: string 
    Email: string
    PhoneNumber: string   
    Password: string
  }

  const AuthScreen:FC =() => {
    //хук
    const {signed, user, signIn, loading} = useContext(AuthContext)
    const [data, setData] = useState<IData>({} as IData)
    const [token, setToken] = useState('');

    //состояния (зарегистрирован или нет)
    const [isReg, setIsReg] = useState(false)

    console.log(signed)
    console.log(user)

    function handleSignIn(){
        console.log('login')
        console.log('data.email:', data.Email)
        console.log('data.password:', data.Password)
        signIn(data.Email, data.Password).then((data) => {setToken(data.token); console.log("data.token", data.token)});

        
    }
    function handleSignUp(){
        console.log('reg')
        console.log('data.FirstName:', data.FirstName)
        console.log('data.LastName:', data.LastName)
        console.log('data.email:', data.Email)
        console.log('data.PhoneNumber:', data.PhoneNumber)
        console.log('data.password:', data.Password)
        signUp(data.FirstName, data.LastName, 
            data.Email, data.PhoneNumber, data.Password);
    }

  
  
    const authHandler = async() =>{

      if(isReg) await handleSignUp()
      else await handleSignIn()
  
      setData({} as IData)
  
    }
  
      return(
      <View style = {styles.container}>
        <View style={styles.wrapper}>
        {loading ? <Loader /> :
          <>
          <View style={styles.sectionContainer}>
          <Text style = {styles.text}>
            {isReg ? 'Регистрация' : 'Вход в аккаунт'}
          </Text>
          </View>
          {isReg? 
          <View>
            <Field val={data.FirstName} 
          placeholder = 'Введите имя' 
          onChange={val => setData({...data, FirstName: val})}
          /> 
          <Field val={data.LastName} 
          placeholder = 'Введите фамилию' 
          onChange={val => setData({...data, LastName: val})}
          />
          <Field val={data.Email} 
          placeholder = 'Введите свой email' 
          onChange={val => setData({...data, Email: val})}
          />
          <Field val={data.PhoneNumber} 
          placeholder = 'Введите свой телефон' 
          onChange={val => setData({...data, PhoneNumber: val})}
          />
          <Field val={data.Password} placeholder = 'Введите пароль' 
          onChange={val => setData({...data, Password: val})}
          isSecure={true}
          />
          </View>
          :
          <View>
          <Field val={data.Email} 
          placeholder = 'Введите свой email' 
          onChange={val => setData({...data, Email: val})}
          />
          <Field val={data.Password} placeholder = 'Введите пароль' 
          onChange={val => setData({...data, Password: val})}
          isSecure={true}
          />    
          </View>
           }
              
  
          <Button onPress={authHandler} title={'Ввод'} 
          color={'#fac7c3'}/>
  
          <Pressable onPress={() => setIsReg(!isReg)}>
          <Text style={styles.text}>
              {isReg ? 'Уже есть аккаунт?' : 'Еще нет аккаунта?'}
            </Text>
            <Text style={styles.text}>
              {isReg ? 'Войти' : 'Зарегистрироваться'}
            </Text>
          </Pressable>
          </>
          }
        </View>   
      </View>
      )
  }
  
  export default AuthScreen
  
    const styles = StyleSheet.create({
      container: {
        flex: 1,
        alignItems: 'center',
        justifyContent: 'center',
      },
      wrapper: {
        width: '80%',
      },
      text: {
        color: '#fac7c3',
        flexDirection: 'row',
        marginTop: 20,
        fontSize: 24,
        fontWeight: 'bold',
        textAlign: 'center',
        
      },
      sectionContainer:{
        justifyContent: 'center',
        alignItems: "center",
        paddingHorizontal: 24,
        paddingVertical: 20
      },
  
    
    });

function signUp(FirstName: string, LastName: string, Email: string, PhoneNumber: string, Password: string) {
    const request:any = {
        FirstName: FirstName,
        LastName: LastName,
        Email: Email,
        PhoneNumber: PhoneNumber,  
        Password: Password
      };
      console.log("request: ", request)
      const formBody = [];
      for (let property in request) {
        const encodedKey = encodeURIComponent(property);
        const encodedValue = encodeURIComponent(request[property]);
        formBody.push(encodedKey + "=" + encodedValue);
      }
      console.log("form body: ", formBody)
      
      const formBodyStr = formBody.join("&");
      console.log("formBodyStr : ", formBodyStr)
    const res = fetch('http://localhost:5000/connect/register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/form-data'
      },
      body: formBodyStr
    }).then(res => console.log(res));
}
