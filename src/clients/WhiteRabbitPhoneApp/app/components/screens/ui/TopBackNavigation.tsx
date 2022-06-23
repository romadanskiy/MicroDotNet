import { useNavigation } from "@react-navigation/native"
import React from "react"
import { TouchableHighlight, View, StyleSheet} from "react-native"
import BackIcon from "./BackIcon"


const TopBackNavigation = () => {
    const navigation = useNavigation()

    return(
        <View style={styles.container}>
            <TouchableHighlight 
            underlayColor="#f0ddcc"
            style={styles.backButton}
            onPress={() => {
                navigation.goBack();
            }}>
                <BackIcon color="#3333" size={20} />

            </TouchableHighlight>
        </View>
    )

}

const styles = StyleSheet.create({
    container: {
        flexDirection: "row"
    },
    backButton: {
        borderRadius: 8,
        width: 40,
        height: 40,
        justifyContent: "center",
        alignItems: "center"
    }
})

export default TopBackNavigation