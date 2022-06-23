import React, { FC } from "react";
import {Text, TouchableHighlight, StyleSheet } from "react-native";

interface IButton{
    //принимает val отдает void
    onPress: () => void
    title: string
    //необязательное поле
    //один цвет при наведении, второй при клике
    colors?: [string, string]

}

//принимает интерфейс IButton
const Button:FC<IButton> =({onPress, title, colors = ['FBBF24','FBBF20']}) => {
    return(
       <TouchableHighlight 
       onPress={onPress} 
       underlayColor={colors[1]}
       style={styles.box}
       >
           <Text style={styles.text}>{title}</Text>

       </TouchableHighlight>
    )
}

export default Button

const styles = StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: '#fff',
      alignItems: 'center',
      justifyContent: 'center',
    },
    text: {
      color: 'red'
    },
    box: {
      //backgroundColor: "#fac7c3",
      flexDirection: 'row',
      justifyContent: 'space-around',
      width: 400,
      height: 70
    },
  
  });