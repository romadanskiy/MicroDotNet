import React, { FC } from "react";
import { View, Text, StyleSheet } from "react-native";
import TopDrawerNavigation from "../../../navigation/TopDrawerNavigation";

const Services:FC =() => {
    return(
        <View>
            <TopDrawerNavigation />
            <Text style={styles.screenTitle}>Services Screen</Text>
            <Text>
            Services
            </Text>
        </View>
    )
}

export default Services

const styles = StyleSheet.create({
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
        fontSize: 24,
        marginTop: 8,
        fontWeight: 'bold',
    },

});