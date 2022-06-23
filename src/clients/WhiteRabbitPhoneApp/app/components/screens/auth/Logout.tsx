import React, { FC, useContext } from "react";
import { View, StyleSheet, Button } from "react-native";
import { useAuth } from "../../../hooks/useAuth";
import Header from "../../layout/Header";
import { Text } from '@rneui/themed';
import AuthContext from "./contexts/auth";

const LogoutScreen:FC =() => {
    /*
    const {signOut} = useContext(AuthContext)
    function handleSignOut() {
        signOut();
    }
    */
    
    const {logout} = useAuth()
    const logoutHandler = async() =>{
        logout()
      }

    return(
        <View style={styles.mainContainer}>
            <Header />
            <View style={styles.container}>
                <View style={styles.content}>
                    <Text style={styles.text}>Вы действительно хотите выйти из аккаунта?</Text>
                </View>
                <View style={styles.sectionContainer}>
                <Button onPress={logoutHandler} title={'Да'} 
                color={'#fac7c3'}/>

                </View>
            </View>

        </View>
    )
}

export default LogoutScreen

const styles = StyleSheet.create({
    mainContainer: {
        flex: 1,
    },
    container: { 
        justifyContent: "center",
        alignItems: "center",
    },
    sectionContainer:{
        marginTop: 32,
        paddingHorizontal: 24,
        width: '80%',
    },
    content:{
        justifyContent: "center",
        alignItems: "center",
        marginTop: 150,
    },
    screenTitle: {
        fontSize: 24,
        marginTop: 8,
        fontWeight: 'bold',
    },
    text: {
      flexDirection: 'row',
      marginTop: 20,
      fontSize: 24,
      fontWeight: 'bold',
      textAlign: 'center',
      padding: 5,
      
    },

});