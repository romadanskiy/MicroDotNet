import React, { FC } from "react";
import {TextInput, StyleSheet } from "react-native";

interface IField{
    //принимает val отдает void
    onChange: (val: string) => void
    //onChange: (num: number) => void
    val: string
    //num?: number
    placeholder: string
    //необязательное поле
    isSecure?: boolean
    defaultVal?: string

}

//принимает интерфейс IField
const Field:FC<IField> =({onChange, placeholder, val, isSecure, defaultVal}) => {
    return(
       <TextInput 
       //чтобы клавиатура не мешала
       //showSoftInputOnFocus={false}
       style={styles.input}
       defaultValue = {defaultVal}
       placeholder={placeholder} 
       onChangeText={onChange} 
       value={val}
       secureTextEntry={isSecure}/>
    )
}

export default Field

const styles = StyleSheet.create({ 
    input: {
        height: 40,
        marginBottom: 12,
        borderWidth: 1,
        borderColor: '#bbb',
        borderRadius: 5,
        paddingHorizontal: 14,
      },

});