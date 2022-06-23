import {initializeApp} from 'firebase/app'
import {
    getAuth,
    signOut,
    signInWithEmailAndPassword,
    createUserWithEmailAndPassword,
} from 'firebase/auth'
import {getFirestore} from '@firebase/firestore'

const firebaseConfig = {
    apiKey: "AIzaSyCDFQIM0d19C9EFPFqUTwgNM_OP-mZ_S-c",
  authDomain: "white-rabbit-6f75c.firebaseapp.com",
  projectId: "white-rabbit-6f75c",
  storageBucket: "white-rabbit-6f75c.appspot.com",
  messagingSenderId: "407167319023",
  appId: "1:407167319023:web:053cc36efefa506656941a"

}

initializeApp(firebaseConfig)

export const auth = getAuth()

export const register = (email: string, password: string) => 
createUserWithEmailAndPassword(auth, email, password)

export const login = (email: string, password: string) => 
signInWithEmailAndPassword(auth, email, password)

export const logout = () => signOut(auth)

//для запросов к бд
export const db = getFirestore()
