import React, { FC } from "react";
import { View, Text, ScrollView, StyleSheet } from "react-native";
import {useTailwind} from 'tailwind-rn';


interface ILayout{
    //необходимо чтобы header оставался статичным
    isScrollView?: boolean

}
//tailwind на андроид не работает
export const tailwind = useTailwind();

export const styleCenter = tailwind('h-full w-full bg-white pt-16')
//обертка
const Layout:FC<ILayout> =({children, isScrollView = true}) => {
    return(
        <View style={styleCenter}>
            {isScrollView ? <ScrollView>{children}</ScrollView> : children}
        </View>
    )
}

export default Layout
