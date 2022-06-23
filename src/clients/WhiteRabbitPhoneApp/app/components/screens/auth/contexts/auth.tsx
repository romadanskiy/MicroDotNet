import { createContext, FC, useEffect, useState } from "react";
import * as AuthOIDC from '../authOIDC'
import AsyncStorage from '@react-native-async-storage/async-storage'
import { ActivityIndicator, View } from "react-native";

interface User {
    FirstName: string
    LastName: string 
    Email: string
    PhoneNumber: string   
    Password: string
    //name: string
    //email: string
}
interface AuthContextData{
    signed: boolean
    token: string
    user: User | null
    loading: boolean
    signIn(Email: string, Password: string): Promise<AuthOIDC.Response>;
    signOut(): void;
}

const AuthContext = createContext<AuthContextData>({} as AuthContextData)

export const AuthProvider: FC = ({children}) => {

    const [user, setUser] = useState<User | null>(null)
    const [loading, setLoading] = useState(false)

    const [signed, setSigned] = useState(false)
    useEffect(() => {
        async function loadStoragedData() {

            setLoading(true)
            
            const storagedUser = await AsyncStorage.getItem('@RNAuth: user')
            const storagedToken = await AsyncStorage.getItem('@RNAuth: token')

            //await new Promise(resolve => setTimeout(resolve, 2000));

            if (storagedUser && storagedToken){
                //api.defaults.headers.Authorization = `Bearer ${storagedToken}`
                //setUser(JSON.parse(storagedUser));
                setLoading(false)
            }else{
                setLoading(false)
            }
            
        }
        loadStoragedData();
    },[])

    //Функция  авторизации
    async function signIn(Email: string, Password: string) {
        const response = await AuthOIDC.signIn(Email, Password);
        console.log("response is auth", response);
        setSigned(true)
        return response
        //setUser(response.user)

        //api.defaults.headers['Authorization'] = `Bearer ${response.token}`
        //const {token, user} = response

        //await AsyncStorage.setItem('@RNAuth: user', JSON.stringify(response.user))
        //await AsyncStorage.setItem('@RNAuth: token', response.token)
    }
    function signOut() {
        AsyncStorage.clear().then(() => {
            setSigned(false)
            //setUser(null)
        })
    }
    return(
        <AuthContext.Provider value={{signed, loading, token: '', user, signIn, signOut}}>
        {children}
    </AuthContext.Provider>

    )
}

export default AuthContext

//signed: !!user