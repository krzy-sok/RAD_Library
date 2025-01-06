import React from 'react';
import { useFormContext } from "react-hook-form";


export const InputField = ({ label, id, defaultVal, type }) => {
    const { register } = useFormContext();

    return (
        <div className="form-group" >
            <label htmlFor={id} > {label} </label>
            < input type={type} id={id} name={id} placeholder={defaultVal} className="form-control"
                {...register(id, {
                    required: {
                        value: true,
                        message: `${label} is required`,
                    }
                })
                }
            />
        </div>
    );
};