import { NativeStackNavigationProp, NativeStackScreenProps } from "@react-navigation/native-stack";
import React, { FC, useCallback, useEffect, useState } from "react";
import { View, StyleSheet, ScrollView, Image, RefreshControl} from "react-native";
import { RootStackParamList } from "../../../navigation/types";
import Header from "../../layout/Header";
import { Text } from '@rneui/themed';
import { TouchableOpacity } from "react-native-gesture-handler";
import { useNavigation } from "@react-navigation/native";
import { AnimalType} from "../../Api/post.interface";
import { Animal} from "../../Api/api";
import AnimalCard from "./AnimalCard";
import Loader from "../ui/Loader";

type Props = NativeStackScreenProps<RootStackParamList, "HomeStack">;

const Home = ({navigation}: Props) => {
    const [isCreate, setIsCreate] = useState(false);
	const [isEdit, setIsEdit] = useState(false);
    const [isLoading, setIsLoading] = useState(false)
    const [refreshing, setRefreshing] = useState(false);
    //add
    const [animals, setAnimals] = useState<AnimalType[]>([]);

	const [isError, setIsError] = useState<boolean>(false);
    /*
    const wait = (timeout: any) => {
        return new Promise(resolve => setTimeout(resolve, timeout));
      }
      */
    const onRefresh = useCallback(() => {
        setRefreshing(true);
        Animal.getPosts()
			.then((data) => {
				setAnimals(data);
			}).then(() => setRefreshing(false))
			.catch((err) => {
				setIsError(true);
			})
      }, []);
     //add
	useEffect(() => {
        setIsLoading(true)
		Animal.getPosts()
			.then((data) => {
				setAnimals(data);
			}).then(() => setIsLoading(false))
			.catch((err) => {
				setIsError(true);
			});
		return () => {};
	}, []);
    //const navigation = useNavigation<NativeStackNavigationProp<HomeStackParam>>()
    return(
        <View style={styles.mainBlock}>
            <Header />
            {isLoading ? <Loader /> :
            <>
            <View style={styles.box}>
            <Text style={styles.screenTitle}>Главная</Text>
            </View>
            
            <View style={styles.content}>
            <ScrollView
            refreshControl={
                <RefreshControl
                  refreshing={refreshing}
                  onRefresh={onRefresh}
                />}
            > 
                <View style={styles.container}>
                    {animals.map((animal) => {
                        return(
							<AnimalCard
								key={animal.id}
                                id = {animal.id}
								animal={animal}
                                avatar={animal.avatarUrl}
                                name = {animal.name}
                                age = {animal.age}
                                onLiked = {animal.is_liked}
                                onBooked= {animal.is_booked}
                                onPress={(name, avatar, onLiked, onBooked, id) => {
                                    navigation.navigate("AnimalScreen", {name, avatar, onLiked, onBooked, id});
                                }}
							/>
						)})
                    }
                </View>
            </ScrollView>
            </View>
            </>
            }
            
        </View>
    )
}

export default Home

const styles = StyleSheet.create({
    mainBlock: {
        flex: 1,
        //backgroundColor: "blue"
    },
    box: {
        flex: 1,
        justifyContent: "center",
        alignItems: "center",
        //backgroundColor: "yellow"
    },
    sectionContainer:{
        marginTop: 32,
        paddingHorizontal: 24,
        paddingTop: 10,
    },
    content:{
        flex: 10,
    },
    screenTitle: {
        fontSize: 24,
        marginTop: 8,
        fontWeight: 'bold',
        alignItems: "center",
        
    },
    ///
    container: {
        flex: 1,
        flexWrap: 'wrap',
        flexDirection: 'row'
      },
      image: {
        width: 150,
        height: 140,
      },
      name: {
        fontSize: 16,
        marginTop: 5,
      },

});