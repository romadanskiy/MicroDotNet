import React, { FC } from "react";
import { View, StyleSheet, TextInput, ScrollView } from "react-native";
import EditProfileHeader from "./EditProfileHeader";
import { Avatar, Text, useTheme, Button  } from '@rneui/themed';
import { useNavigation } from "@react-navigation/native";
import { NativeStackNavigationProp } from "@react-navigation/native-stack";
import { ProfileStackParam } from "../../../navigation/types";

const EditProfile:FC =() => {
    const navigation = useNavigation<NativeStackNavigationProp<ProfileStackParam>>()

    return(
        <View style={styles.mainBlock}>
            <EditProfileHeader/>
            <ScrollView>         
            <View style={styles.container}>
                <Text style={styles.text} h3>Телефон</Text>
                <TextInput placeholder="Новый номер" style={styles.input}/>
                <Text style={styles.text} h3>Город</Text>
                <TextInput placeholder="Новый город" style={styles.input}/>
                <Text style={styles.text} h3>Эл. почта</Text>
                <TextInput placeholder="Новая почта" style={styles.input}/>
                <Text style={styles.text} h3>Дата рождения</Text>
                <TextInput placeholder="Новая дата рождения" style={styles.input}/>
                <Text style={styles.text} h3>Пол</Text>
                <TextInput placeholder="Новый пол" style={styles.input}/>
                

            </View>
            <View style={styles.buttonsContainer}>
                <Button
                onPress={() => navigation.navigate("Profile")}
                title="Сохранить"
                buttonStyle={{
                backgroundColor: '#fac7c3',
                borderRadius: 5,
              }}
              containerStyle={{
                width: 250,
                marginHorizontal: 50,
              }}
            />
            </View>
                
            </ScrollView>
        
            
        </View>
    )
}

export default EditProfile

const styles = StyleSheet.create({

    mainBlock: {
        flex: 1,
        //backgroundColor: "blue"
    },
    container: {
        padding: 16,

    },
    sectionContainer:{
        marginTop: 32,
        paddingHorizontal: 24,
    },
    content:{
        justifyContent: "center",
        alignItems: "center",
        //backgroundColor: "red",
        marginTop: 150,

    },
    screenTitle: {
        marginTop: 8,
        fontWeight: 'bold',
    },
    input: {
        height: 40,
        margin: 12,
        borderWidth: 2,
        padding: 10,
      },
      text: {
        padding: 5,
      },
      buttonsContainer: {
        justifyContent: 'center',
        alignItems: 'center',
        width: '100%',
        marginVertical: 20,
      },

});