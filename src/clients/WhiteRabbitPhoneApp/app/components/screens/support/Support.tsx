import React, { FC } from "react";
import { View, Text, StyleSheet } from "react-native";
import TopDrawerNavigation from "../../../navigation/TopDrawerNavigation";

const Support:FC =() => {
    return(
        <View>
            <TopDrawerNavigation />
            <Text style={styles.screenTitle}>Support Screen</Text>
            <Text>Support</Text>
        </View>
    )
}

export default Support

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