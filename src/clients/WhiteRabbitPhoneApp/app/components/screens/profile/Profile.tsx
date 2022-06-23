import React, { FC } from "react";
import { View, StyleSheet, ScrollView,  } from "react-native";
import Header from "../../layout/Header";
import { Avatar, Text, useTheme, Button  } from '@rneui/themed';

import { NativeStackNavigationProp, NativeStackScreenProps } from "@react-navigation/native-stack";

import { useNavigation } from "@react-navigation/native";
import { ProfileStackParam } from "../../../navigation/types";
import { useAuth } from "../../../hooks/useAuth";

type TextComponentProps = {};

const Profile:FC<TextComponentProps> =() => {
    const navigation = useNavigation<NativeStackNavigationProp<ProfileStackParam>>()
    const { theme } = useTheme();
    const {user} = useAuth()
    return(
        <View style={styles.container}>
            <Header />
            <ScrollView>
            <View
            style={{
                flexDirection: 'row',
                justifyContent: 'space-around',
                marginBottom: 40,
                marginTop: 30,
                }}>
                <Avatar
                size={150}
                rounded
                source={{ 
                    uri: 'https://avatars.design/wp-content/uploads/2016/09/avatar1b.jpg'
                    //{l.image_url ? { uri: l.image_url } : {}} 
                }}
                title="Bj"
                containerStyle={{ backgroundColor: 'grey' 
                }}>
                <Avatar.Accessory size={40} />
                </Avatar>
            </View>
            <Text style={styles.screenTitle} h3>Иван Петров</Text>
            <View style={styles.view}>
                <Text
                style={styles.text}
                h3
                h3Style={{ color: theme?.colors?.warning }}
                >
                    Почта:
                </Text>
                <Text
                style={styles.text}
                h4
                >
                    {user?.email}
                </Text>
                <Text
                style={styles.text}
                h3
                h3Style={{ color: theme?.colors?.warning }}
                >
                    Номер:
                </Text>
                <Text
                style={styles.text}
                h4
                >
                    88552853535 
                </Text>
                <Text
                style={styles.text}
                h3
                h3Style={{ color: theme?.colors?.warning }}
                >
                    Пол:
                </Text>
                <Text
                style={styles.text}
                h4
                >
                    мужской
                </Text>
                <Text
                style={styles.text}
                h3
                h3Style={{ color: theme?.colors?.warning }}
                >
                    Город:
                </Text>
                <Text
                style={styles.text}
                h4
                >
                    Гордый
                </Text>
                <Text
                style={styles.text}
                h3
                h3Style={{ color: theme?.colors?.warning }}
                >
                    Дата рождения:
                </Text>
                <Text
                style={styles.text}
                h4
                >
                    22.02.1978
                </Text>
            </View>
            <View style={styles.buttonsContainer}>
                <Button
                onPress={() => navigation.navigate("EditProfile")}
                title="Редактировать"
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

const styles = StyleSheet.create({
    container:{
        flex: 1,
    },
    screenTitle:{
        fontSize: 24,
        fontWeight: 'bold',
        textAlign: 'center',
    },
      view: {
        margin: 20,
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

})

export default Profile