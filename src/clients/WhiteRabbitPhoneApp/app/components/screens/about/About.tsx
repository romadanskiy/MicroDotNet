import React, { FC } from "react";
import { View, StyleSheet } from "react-native";
import TopDrawerNavigation from "../../../navigation/TopDrawerNavigation";
import Header from "../../layout/Header";
import { Text, Image, Card  } from '@rneui/themed';
import { ScrollView } from "react-native-gesture-handler";

const AboutScreen:FC =() => {
    return(
        <View style={styles.container}>
            <Header />
            <ScrollView>
            <View style={styles.view}>
            <Text style={styles.screenTitle} h1>О нас</Text>
                <Text style={styles.screenTitle} h4>
                    Помогать просто!
                </Text>
                <Text style={styles.screenTitle} h4>
                    Если вы хотите помочь приюту для животных, но пока не готовы взять домой кошку или собаку — можно поддержать его иначе. 
                </Text>
                <Text style={styles.screenTitle} h4>
                    Это не обязательно должна быть финансовая помощь: в подобных местах часто не хватает самых обычных вещей.
                    Узнайте какие приюты есть в вашем городе.
                </Text>
            </View>
            <View style={styles.view}>
            <Image
            style={styles.image}
            source={{ uri: 'https://static-maps.yandex.ru/1.x/?api_key=01931952-3aef-4eba-951a-8afd26933ad6&theme=light&lang=ru_RU&size=520%2C440&l=map&spn=0.008210%2C0.004642&ll=49.205022%2C55.654833&lg=0&cr=0&pt=49.205022%2C55.654833%2Ccomma&signature=TQwQM3JLkxnIKT6XQ29L9Q2Rt5J1qsIq_njDulSW-QE=' }}
            />
            </View>
            <View>
            <Text style={styles.screenTitle} h4>
                    Приют бездомных животных в поселке Столбище.
            </Text>
            <Text style={styles.screenTitle} h4>
                   Приют работает более 19 лет, и за это время его сотрудники нашли дом более чем для 500 собак, не усыпляет животных, а ухаживает за ними, оказывает необходимый уход и лечение.
            </Text>
            <Text style={styles.screenTitle} h4>
                   Телефон: 88005553535
            </Text>
            </View>
            </ScrollView>
        </View>
    )
}

export default AboutScreen

const styles = StyleSheet.create({
    container:{
        flex: 1,
     },
    sectionContainer:{
        marginTop: 32,
        paddingHorizontal: 24,
    },
    view: {
        margin: 20,
      },
      text: {
        margin: 20,
      },
      image: {
        aspectRatio: 1,
        width: '100%',
        flex: 1,
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