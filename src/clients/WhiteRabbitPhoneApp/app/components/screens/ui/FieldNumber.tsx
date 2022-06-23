import React, { FC } from "react";
import NumericInput from "react-native-numeric-input";

interface IFieldNumber{
    onChange: (val: number) => void
    val: number
}

const FieldNumber:FC<IFieldNumber> =({onChange, val}) => {
    return(
       <NumericInput 
       onChange={onChange}
       value={val}
        totalWidth={385} 
        totalHeight={40} 
        step={1}
        valueType='real'
        rounded
        rightButtonBackgroundColor='#E56B70' 
        leftButtonBackgroundColor='#E56B70'
       />
    )
}

export default FieldNumber
