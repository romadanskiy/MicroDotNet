import React from 'react'
import { View, StyleSheet, Text } from 'react-native'
import { TouchableHighlight } from 'react-native-gesture-handler'
//import BackIcon from '../icons/BackIcon'
import { useNavigation } from '@react-navigation/core'
import DrawerMenuIcon from '../components/screens/icons/DrawerMenuIcon'
import { DrawerNavigationProp } from '@react-navigation/drawer'
import { RootStackParamList } from './types'

const TopDrawerNavigation = () => {
  const navigation = useNavigation<DrawerNavigationProp<RootStackParamList>>()

  return (
  <View style={styles.container}>
    <TouchableHighlight style={styles.backButton} underlayColor="#f0ddcc" onPress={() => {
      navigation.openDrawer();
    }}>
      <DrawerMenuIcon color="#333" size={35} />
    </TouchableHighlight>
  </View>
  )}

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row'
  },
  backButton: {
    borderRadius: 8,
    width: 40,
    height: 40,
    justifyContent: "center",
    alignItems: "center"
  }
})

export default TopDrawerNavigation
