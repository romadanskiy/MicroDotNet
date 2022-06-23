import React, { FC, useCallback, useEffect, useState } from "react";
import { Text, StyleSheet, View, Image, ScrollView, RefreshControl } from "react-native";
import Header from "../../layout/Header";
import { AnimalType } from "../../Api/post.interface";
import { Animal } from "../../Api/api";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { RootStackParamList } from "../../../navigation/types";
import AnimalCard from "../home/AnimalCard";
import Loader from "../ui/Loader";

type Props = NativeStackScreenProps<RootStackParamList, "HomeStack">;
const Booked=({navigation}: Props) => {
   //add
   const [isEdit, setIsEdit] = useState(false);
   const [animals, setAnimals] = useState<AnimalType[]>([]);
   const [isError, setIsError] = useState<boolean>(false);

   const [isLoading, setIsLoading] = useState(false);
   const [refreshing, setRefreshing] = useState(false);

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
    return(
      <View style={styles.mainBlock}>
      <Header />
      {isLoading ? <Loader /> :
      <>
      <View style={styles.box}>
      <Text style={styles.screenTitle}>Забронировано</Text>
      </View>
      <View style={styles.content}>
      <ScrollView
      refreshControl={
        <RefreshControl
          refreshing={refreshing}
          onRefresh={onRefresh}
        />
        }>
          <View style={styles.container}>
              {animals.map((animal) => {
                  if(animal.is_booked== true){
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
                      )
                  }
                  })
              }
          </View>
      </ScrollView>   
      </View>
      </>
      }
      
  </View>
    )
}

const styles = StyleSheet.create({
  mainBlock: {
      flex: 1,
      //backgroundColor: "blue"
  },
  container: {
      flex: 1,
      flexWrap: 'wrap',
      flexDirection: 'row'
    },
  screenTitle:{
      fontSize: 24,
      marginTop: 8,
      fontWeight: 'bold'
  },
  box: {
      flex: 1,
      justifyContent: "center",
      alignItems: "center",
      //backgroundColor: "yellow"
  },
  content:{
      flex: 10,
  },

})


export default Booked