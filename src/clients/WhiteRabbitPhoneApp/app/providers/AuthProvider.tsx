import React, {createContext, useState, useMemo, useEffect, FC} from 'react'
import { onAuthStateChanged, User } from 'firebase/auth'
import { Alert } from 'react-native'
import { async } from '@firebase/util'
import { addDoc, collection } from '@firebase/firestore'
import {auth, db, login, logout, register } from '../firebase'

interface IContext{
    user: User | null
    isLoading: boolean
    register: (email: string, password: string) => Promise<void>
    login: (email: string, password: string) => Promise<void>
    logout: () => Promise<void>

}

export const AuthContext = createContext<IContext>({} as IContext)
//children получаем из props ов которые реакт передает по дефолту
export const AuthProvider:FC = ({children}) => {

    const [user, setUser] = useState< User | null>(null)
    // чтобы обновлялись страницы, менялось состояние 
    //и авторизованному юзеру не показывалась страница с авторизацией
    const [isLoadingInitial, setIsLoadingInitial] = useState(true)
    const [isLoading, setIsLoading] = useState(false)

    //функция осуществляющая регистрацию
    const registerHandler = async(email: string, password: string) =>{
        setIsLoading(true)
        try {
            const {user} = await register(email, password)

            //таблица с данными юзера (временное бд) 
            const docRef = await addDoc(collection(db, "users"),{
                //какие поля будут добавлены
                _id: user.uid,
                displayName: "No name"
            })
            console.log("Document written with ID: ", docRef.id);

        } catch(error: any){
            //Alert.alert('Error reg: ', error)

        } finally{
            setIsLoading(false)   
        }
    }

    //функция авторизации
    const loginHandler = async(email: string, password: string) => {
        setIsLoading(true)
        try {
            await login(email, password)
        } catch(error: any){
            Alert.alert('Error login: ', error)

        } finally{
            setIsLoading(false)   
        }

    }

     //функция выхода
     const logoutHandler = async() => {
        setIsLoading(true)
        try {
            await logout()
        } catch(error: any){
            Alert.alert('Error logout: ', error)

        } finally{
            setIsLoading(false)   
        }
    }

    //при авторизации автоматическое обновление состояния приложения
    //dependency постое т.к. нужно 1 раз при загрузке страницы
    //onAuthStateChanged слушатель позволяющий следить за авторизацией
    // Load any resources or data that we need prior to rendering the app
    useEffect(
        () => onAuthStateChanged(auth, user => {
            //каждый раз при заходе в систему срабатывает эта функция
        setUser(user || null)
        //чтобы приложение понимало когда мы проверили авторизацию
        setIsLoadingInitial(false)
    }),[])


    //изменяется при изменении user, isLoading
    const value = useMemo(() => ({
        user, 
        isLoading, 
        login: loginHandler, 
        logout: logoutHandler, 
        register: registerHandler
    }), [user, isLoading])

    return <AuthContext.Provider value={value}>
        {!isLoadingInitial && children}
    </AuthContext.Provider>
}