import React, { useContext } from 'react'
import AuthContext from './contexts/auth'


export const useAuthOIDC = () => {
    const context  = useContext(AuthContext)

    return context
}