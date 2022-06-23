import React, { FC } from "react";
import { View, Text, StyleSheet, Image } from "react-native";
import TopDrawerNavigation from "../../navigation/TopDrawerNavigation";


//обертка
const Header =() => {
    return(
        <View style={styles.mainBlock}>
            <View style={styles.box}>
                <View style={styles.content}>
                    <Image style={styles.tinyLogo}
                    source={require('../../images/Rabbit.png')} />
                    <Text style={styles.screenTitle}>White Rabbit</Text>
                </View>
                
                <TopDrawerNavigation />
            </View>
        </View>
    )
}

export default Header

const styles = StyleSheet.create({
    mainBlock: {
        paddingTop: 25,
        backgroundColor: "#fac7c3",
        height: 130,
    },
    box: {
        flex: 1,
    },
    screenTitle: {
        fontSize: 24,
        marginTop: 8,
        fontWeight: 'bold',
    },
    content:{
        paddingRight: 40,
        justifyContent: "center",
        flexDirection: "row",
        marginTop: 9,

    },
    tinyLogo: {
        width: 50,
        height: 50,
      },
});