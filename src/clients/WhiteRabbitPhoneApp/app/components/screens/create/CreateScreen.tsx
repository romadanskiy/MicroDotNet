import { useNavigation } from "@react-navigation/native";
import { Button } from "@rneui/themed";
import React, { FC, useState } from "react";
import { View, Text, StyleSheet, ScrollView } from "react-native";
import TopDrawerNavigation from "../../../navigation/TopDrawerNavigation";
import { Animal } from "../../Api/api";
import { AnimalType } from "../../Api/post.interface";
import Header from "../../layout/Header";
import Field from "../ui/Field";
import FieldNumber from "../ui/FieldNumber";

interface IData {
    id: number
    name: string
    age: number
    about: string
    address: string
    avatarUrl: string
    is_booked: boolean
    is_liked: boolean
  }

const CreateScreen:FC =() => {
     //хук
 // const {isLoading, login, register} = useAuth()
  const [animals, setAnimals] = useState<AnimalType[]>([]);
  const [data, setData] = useState<IData>({} as IData)
  const [isCreate, setIsCreate] = useState(false)
  const navigation = useNavigation()

  const onCheckNum = (value: string) => {
    const parsedQty = Number.parseInt(value)
    console.log("число:", parsedQty)
    if (Number.isNaN(parsedQty)) {
      //setQuantity(0) //setter for state
      
      setData({...data, age: parsedQty})
    } 
     else {
      //setQuantity(parsedQty)
      setData({...data, age: 0})
    }
  }


  const CreateHandler = async() =>{
    //эмайл из дата
    const {name, age, about, address, avatarUrl, is_booked, is_liked} = data

     //функция осуществляющая регистрацию
        setIsCreate(true)
        try {
            Animal.createPost(data)
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

    setData({} as IData)
    navigation.goBack();

  }
    return(
        <View style={styles.main}>
            <Header />
            <ScrollView> 
            <View style={styles.container}>
            <Text style={styles.screenTitle}>Добавить питомца</Text>
                
                <Text style={styles.text}>Имя питомца</Text>
                 <Field val={data.name}
                 placeholder = 'Введите имя питомца' 
                 onChange={val => setData({...data, name: val})}
                 />
                 <Text style={styles.text}>Возраст</Text>
                 <FieldNumber val={data.age}
                 onChange={val => setData({...data, age:val })}
                 />
                 <Text style={styles.text}>Фотография</Text>
                 <Field val={data.avatarUrl}
                 defaultVal = "https://zooclub.ru/attach/34000/34293.jp"
                 placeholder = 'Введите ссылку на фотографию питомца'
                 onChange={val => setData({...data, avatarUrl: val})}
                 />
                 <Text style={styles.text}>Описание</Text>
                 <Field val={data.about}
                 placeholder = 'Введите описание (не обязательно)'
                 onChange={val => setData({...data, about: val})}
                 />
                 <Text style={styles.text}>Адрес</Text>
                 <Field val={data.address}
                 placeholder = 'Введите адрес (не обязательно)'
                 onChange={val => setData({...data, address: val})}
                 />
                </View>
                <Button style={styles.button}
                onPress={CreateHandler} title={'добавить'} 
                buttonStyle={{
                    backgroundColor: '#fac7c3',
                    borderRadius: 5,
                  }}
                  containerStyle={{
                    width: 250,
                    marginHorizontal: 90,
                  }}
                
                />
            </ScrollView> 
            
            
        </View>
    )
}

export default CreateScreen

const styles = StyleSheet.create({
    main: {
        flex: 1

    },
    container:{
        padding: 16,
        marginTop: 10,
        paddingHorizontal: 24,
    },
    content:{
        justifyContent: "center",
        alignItems: "center",
        //backgroundColor: "red",
        marginTop: 150,

    },
    screenTitle: {
        color: '#fac7c3',
        fontSize: 30,
        fontWeight: 'bold',
        textAlign: 'center',
    },
    text: {
        color: '#fac7c3',
        flexDirection: 'row',
        marginTop: 20,
        fontSize: 24,
        fontWeight: 'bold',
        textAlign: 'center',
        
      },
    button: {
        backgroundColor: '#fac7c3',
        borderRadius: 5,
        width: 250,
        marginHorizontal: 50,

    }

});

