import { async } from "@firebase/util";
import React, { FC, useState } from "react";
import { View, Text, StyleSheet, Button, Pressable } from "react-native";
import { useAuth } from "../../../hooks/useAuth";
import { User } from "../../Api/api";
import { UserType } from "../../Api/post.interface";
import Field from "../ui/Field";
import Loader from "../ui/Loader";

//Auth для firebase
interface IData {
  email: string
  password: string
}

interface IUserApiData {
  Id: number
  firstname: string
	lastname: string
  email: string
  phonenumber: string
  gender: string
  city: string
  date_of_birth: string
  avatarUrl: string
}
/*
const CreateUserHandler = async() =>{
  //эмайл из дата
  const {Id, firstname, lastname, email, phonenumber, gender,
    city, date_of_birth, avatarUrl} = userApiData

   //функция осуществляющая регистрацию
      setIsCreate(true)
      try {
          User.createPost(userApiData)
          .then((data) => {
            console.log('getting data from fetch', data);
          });

      } catch(error: any){
          //Alert.alert('Error reg: ', error)
          console.log("Ошибка при создании", {error})

      } finally{
          setIsCreate(false)
          console.log("объект создан")
      }



  //if(isReg) await register(email, password)
  //else await login(email, password)

  setData({} as IUserApiData)

}
*/
const Auth:FC =() => {
  //users
  const [users, setUsers] = useState<UserType[]>([]);
  const [userApiData, setUserApiData] = useState<IUserApiData>({} as IUserApiData)
  const [isCreate, setIsCreate] = useState(false)

  //animals
  //хук
  const {isLoading, login, register} = useAuth()

  const [data, setData] = useState<IData>({} as IData)
  //состояния (зарегистрирован или нет)
  const [isReg, setIsReg] = useState(false)

  const authHandler = async() =>{
    const {email, password} = data

    if(isReg) await register(email, password)
    else await login(email, password)

    setData({} as IData)

  }

    return(
    <View style = {styles.container}>
      <View style={styles.wrapper}>
      {isLoading ? <Loader /> :
        <>
        <View style={styles.sectionContainer}>
        <Text style = {styles.text}>
          {isReg ? 'Регистрация' : 'Вход в аккаунт'}
        </Text>

        </View>      
        <Field val={data.email} 
        placeholder = 'Введите свой email' 
        onChange={val => setData({...data, email: val})}
        />
        <Field val={data.password} placeholder = 'Введите пароль' 
        onChange={val => setData({...data, password: val})}
        isSecure={true}
        />

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

export default Auth

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