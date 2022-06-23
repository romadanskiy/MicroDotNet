import React, { useEffect, useMemo, useState } from "react"
import { useAuth } from "../../../hooks/useAuth"
import {collection, limit, onSnapshot, query, where} from 'firebase/firestore'
import { db } from "../../../firebase"

interface IProfile {
    _id: string
    displayName: string
    //для запросов связанных с документом
    docId: string

}
export const useProfile = () => {
    const {user} = useAuth()
    //загрузчик который будет срабатывать пока идет запрос на сервер
    const [isLoading, setIsLoading] = useState(true)
    const [profile, setProfile] = useState<IProfile>({} as IProfile)
    //чтобы при обновлении профиля не было пустого имени
    const [name, setName] = useState('')

    //docId не приходит поэтому используем Omit

    useEffect(() => onSnapshot(query(collection(db, 'users'), 
    where('_id', '==', user?.uid), limit(1)), snapshot => {
        const profile = snapshot.docs.map(d => ({
            ...(d.data() as Omit<IProfile, 'docId'>),
            docId: d.id
        }))[0]

        setProfile(profile)
        setName(profile.displayName)
        setIsLoading(false)
    }), [])

    const value = useMemo(() => ({
        profile, isLoading, name, setName
    }), [])

    return value

}