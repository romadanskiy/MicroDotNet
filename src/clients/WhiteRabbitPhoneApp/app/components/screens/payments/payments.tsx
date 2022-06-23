import React, { FC } from "react";
import { View, Text, StyleSheet } from "react-native";
import TopDrawerNavigation from "../../../navigation/TopDrawerNavigation";

const Payments:FC =() => {
    return(
        <View>
            <TopDrawerNavigation />
            <Text style={styles.screenTitle}>Payments Screen</Text>
            <Text>
                Payments
            </Text>
        </View>
    )
}

export default Payments

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