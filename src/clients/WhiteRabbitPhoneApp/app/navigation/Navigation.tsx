import React, { FC, useContext } from "react";
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { ActivityIndicator, Text, View } from "react-native";
import { NavigationContainer } from "@react-navigation/native";
import Auth from "../components/screens/auth/Auth";
import { useAuth } from "../hooks/useAuth";
import Home from "../components/screens/home/Home";
import Profile from "../components/screens/profile/Profile";
import ExploreIcon from "../components/screens/icons/ExploreIcon";
import { createDrawerNavigator } from "@react-navigation/drawer";
import { AuthStackParam, ProfileStackParam, RootStackParamList } from "./types";
import Favorites from "../components/screens/favorites/Favorites";
import Booked from "../components/screens/booked/Booked";
import ProfileIcon from "../components/screens/icons/ProfileIcon";
import LogoutScreen from "../components/screens/auth/Logout";
import EditProfile from "../components/screens/profile/EditProfile";
import AnimalScreen from "../components/screens/home/AnimalScreen";
import AboutScreen from "../components/screens/about/About";
import ApiTest from "../components/screens/apiTest/ApiTest";
import { Icon } from "@rneui/themed";
import CreateScreen from "../components/screens/create/CreateScreen";
import AuthScreen from "../components/screens/auth/AuthScreen";
import AuthContext from "../components/screens/auth/contexts/auth";


const RootStack = createDrawerNavigator<RootStackParamList>()

const AuthStack = createNativeStackNavigator()

export const ProfileStack = createNativeStackNavigator<ProfileStackParam>()

export type HomeStackParam = {
  Home: undefined
  AnimalScreen: undefined//{
    //name: string
    //age: string
    //avatar: string
  //}
}
export const HomeStack = createNativeStackNavigator<HomeStackParam>()

const HomeScreenStack =() => {
  return(
    <HomeStack.Navigator initialRouteName="Home" screenOptions={{
      headerShown: false,
    }}>
      <HomeStack.Screen 
      name='Home'
      component={Home} 
      />
      <HomeStack.Screen 
      name='AnimalScreen'
      component={AnimalScreen}
      />

    </HomeStack.Navigator>

  )
}

const ProfileScreenStack = () => {
  return (
    <ProfileStack.Navigator initialRouteName="Profile" screenOptions={{
      headerShown: false,
    }}>
      <ProfileStack.Screen 
      name='Profile' 
      component={Profile} 
      />
      <ProfileStack.Screen 
      name= "EditProfile" 
      component = {EditProfile} 
      />

    </ProfileStack.Navigator>
  )
}
//<AuthStack.Screen name='Auth' component={Auth} />
//<AuthStack.Screen name='AuthScreen' component={AuthScreen} />  
const AuthScreenStack = () => {
  return (
    <AuthStack.Navigator screenOptions={{
      headerShown: false,
      }}>
        <AuthStack.Screen name='Auth' component={Auth} />
    </AuthStack.Navigator>
    
  )

}
const Navigation:FC = () => {
    const renderContent = () => {
        const {user} = useAuth()
        /*
        const {signed, loading} = useContext(AuthContext)
        if (loading) {
          return(
              <View
              style = {{flex: 1, justifyContent: 'center', alignItems: 'center'}}
              >
                  <ActivityIndicator size="large" color="#666"/>
              </View>
          )
      }
        //const isLoggedIn = false;
        */
        if (user) {
            return (
            <RootStack.Navigator initialRouteName="HomeStack" screenOptions={{
                headerShown: false,
                drawerActiveTintColor: "#FA1003",
                drawerInactiveTintColor: "black",
                }}>
                <RootStack.Screen 
                name='HomeStack' 
                component={HomeScreenStack}
                options = {{
                    drawerIcon: ({color}) => (<Icon
                      color={color}
                      name='home'
                      type='ant-design'
                      />
                    ),
                    drawerLabel: "Главная"
                }}
                />
                <RootStack.Screen 
                name='ProfileStack' 
                component={ProfileScreenStack} 
                options = {{
                    drawerIcon: ({color}) => (<Icon
                      color={color}
                      name='user'
                      type='ant-design'
                      />
                      ),
                    drawerLabel: "Профиль"
                }}
                />
                <RootStack.Screen name='Favorites' component={Favorites} 
                options = {{
                    drawerIcon: ({color}) => (<Icon
                      color={color}
                      name='hearto'
                      type='ant-design'
                      />),
                    drawerLabel: "Понравившиеся"
                }}
                />
                <RootStack.Screen name='Booked' component={Booked} 
                options = {{
                    drawerIcon: ({color}) => (<Icon
                      color={color}
                      name='shield'
                      type='feather'
                      />),
                    drawerLabel: "Забронированные"
                }}
                />
                <RootStack.Screen name='About' component={AboutScreen} 
                options = {{
                    drawerIcon: ({color}) => (<Icon
                      color={color}
                      name='infocirlceo'
                      type='ant-design'
                      />),
                    drawerLabel: "О нас"
                }}
                />
                <RootStack.Screen name='Create' component={CreateScreen} 
                options = {{
                    drawerIcon: ({color}) => (<Icon
                      color={color}
                      name='addfile'
                      type='ant-design'
                      />),
                    drawerLabel: "Добавить питомца"
                }}
                />
                <RootStack.Screen name='ApiTest' component={ApiTest} 
                options = {{
                    drawerIcon: ({color, size}) => (<ExploreIcon color={color} size={size}/>
                    ),
                    drawerLabel: "ApiTest"
                }}
                />   
                <RootStack.Screen name='Logout' component={LogoutScreen} 
                options = {{
                    drawerIcon: ({color}) => (<Icon
                      color={color}
                      name='exit-to-app'
                      type='material-icons'
                      />),
                    drawerLabel: "Выйти"
                }}
                />     
            </RootStack.Navigator>
            )
        }
        return <AuthScreenStack />
    }
    return( 
    <NavigationContainer >
        {renderContent()}
    </NavigationContainer>
    )
}

export default Navigation

