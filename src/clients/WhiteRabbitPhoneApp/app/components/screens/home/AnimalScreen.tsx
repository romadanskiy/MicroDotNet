import { NativeStackScreenProps } from "@react-navigation/native-stack";
import React, { FC, useEffect, useState } from "react";
import { GestureResponderEvent, Pressable, ScrollView, StyleSheet, TouchableHighlight, View } from "react-native";
import { RootStackParamList } from "../../../navigation/types";
import Header from "../../layout/Header";
import EditProfileHeader from "../profile/EditProfileHeader";
import { Text, Card, Button, Icon } from '@rneui/themed';
import { Animal } from "../../Api/api";
import { AnimalType} from "../../Api/post.interface";
import Loader from "../ui/Loader";

type Props = NativeStackScreenProps<RootStackParamList, 'AnimalScreen'>;

const AnimalScreen = ({route}: Props)  => {
    
    const [animals, setAnimals] = useState<AnimalType[]>([]);
    const [isError, setIsError] = useState<boolean>(false);
    const [isLoading, setIsLoading] = useState(false)

    const [value, setValue] = useState({
        id: 0,
		name: '',
        age: 0,
        about: '',
        is_booked: false,
        is_liked: false,
        address: '',
        avatarUrl: ''
	});
//нужно чтобы при первом рендере достать все данные из поста для последующего изменения (put)
//с конкретным id 
	useEffect(() => {
        setIsLoading(true)
		Animal.getAPost(route.params.id!)
			.then((data) =>
				setValue({ ...value, id: data.id, name: data.name, age: data.age, about: data.about,
                    is_booked: data.is_booked, is_liked: data.is_liked, address: data.address, avatarUrl: data.avatarUrl })
			)
            .then(() => setIsLoading(false))
            .then(() => console.log('useEffect with then: ', value))
			.catch((err) => setIsError(true));
		return () => {
            console.log('useEffect 1: ', value);
            
        };
	}, []);
    useEffect(() => {
        console.log('useEffect 2----------------: ');
		console.log('useEffect: ', value);
        //временное решение
        if (value.id != 0) {
            handleSubmit()
        }

		return () => {
        };
	}, [value]);
    

    const handleSubmit = () => {
        //const setAnimals = (updatedPost: AnimalType[]) => {}
		//add
		Animal.updatePost(value, route.params.id!)
			.then((data) => {
                //then = действия после запроса
				//let updatedPost = animals.filter((animal) => animal.id !== route.params.id);
				//setAnimals([data, ...updatedPost]);
				//setValue({ ...value, name: '' });
                console.log('getting data from fetch', data);
			})
			.then((err) => {
				setIsError(true);
			});
	};
    const handleSetLike = () => {
        console.log('value do: ', value.is_liked);
        let localLiked = value.is_liked;
        localLiked = !localLiked
        console.log('localLiked: ', localLiked);

        setValue({ ...value, is_liked: localLiked});
        console.log('value posle: ', value.is_liked);
        //handleSubmit()
    }
    const handleSetBooked = () => {
        console.log('value do: ', value.is_booked);
        let localBooked = value.is_booked;
        localBooked = !localBooked
        console.log('localLiked: ', localBooked);

        setValue({ ...value, is_booked: localBooked});
        console.log('value posle: ', value.is_booked);
        //handleSubmit()
    }
    
    const bodyText = `Зверушка ищет дом. ${value.name} обманчиво создает хорошее впечатление. На самом деле ${route.params.name} любит поиграться, поносить в зубах игрушки. Ждет именно вас, звоните.` 
    return(
        <View style={styles.container}>
            <EditProfileHeader/>
            {isLoading ? <Loader /> :
            <>
            <ScrollView>
            <Card >
                <Card.Image
                style={{ padding: 150}}
                source={{ uri: value.avatarUrl }}
                />
                <Card.Divider />
                <View 
                style={{
                    alignItems: 'center',
                    paddingVertical: 5,
                    flexGrow: 1,
                }}
                >
                   
                <Pressable onPress={(handleSetLike)}>
                { value.is_liked ? 
                
                    <Icon
                    //нет
                    raised
                    name='heart'
                    type='font-awesome'
                    color='#f50'
                    />
                
                : 
                <Icon
                //да
                    raised
                    name='heart-o'
                    type='font-awesome'
                    color='#f50'
                />}
                </Pressable>
                <Card.Divider />
                <Text style={styles.name} h3>{value.name}</Text>
                </View>
                <Card.Divider />
                
                <Text style={styles.baseText}>
                {bodyText}
                </Text>
                <Text style={styles.baseText}>
                    Местонахождение:
                </Text>
                <Text style={styles.baseText}>
                {value.address}
                </Text>
                <Button
                onPress={handleSetBooked}
                buttonStyle={{
                    borderRadius: 0,
                    marginLeft: 0,
                    marginRight: 0,
                    marginBottom: 10,
                    backgroundColor: '#fac7c3'
                }}
                title={value.is_booked ? 'Забронировано' : 'Забронировать'}
                />
        </Card>
                 
            </ScrollView>
            </>
            }
                  
        </View>
    )
}

const styles = StyleSheet.create({
    container:{
       flex: 1,
    },
    screenTitle:{
        fontSize: 24,
        marginTop: 8,
        fontWeight: 'bold'
    },
    name: {
        fontSize: 16,
        //marginTop: 5,
      },
      baseText: {
        fontSize: 20,
        fontWeight: "bold",
        marginBottom: 30

      },
      button: {
        padding: 10,
        marginVertical: 15,
        backgroundColor: '#0645AD'
      },
      buttonText: {
        color: '#fff'
      }

})

export default AnimalScreen