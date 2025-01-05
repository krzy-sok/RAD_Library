import React, { useEffect, useState } from 'react';
import { Header, Footer } from './shared/_Layout'
import { useParams } from 'react-router-dom';
import { Book } from "./Catalogue"
import { useForm, FormProvider, useFormContext } from "react-hook-form";


//interface ValidationErrors {
//    title: string,
//    author: string
//}

export const EditBookForm = (bookId:string, create=false) => {
    const [book, setBook] = useState<Book>()
    useEffect(() => {
        if (!create) {
            getBook(parseInt(bookId!))
        }
        setBook({ title: "", author: "", publisher:"", publicationDate: Date(), price: 0 })

    }, [bookId]);
    const methods = useForm();
    const onSubmit = methods.handleSubmit(data => {
        console.log(data)
        if (create) {
            //call to api /book/bookID with post method
        }
        else {
            //call to api with put method
        }
    })
    
    return (
    book === undefined ? <p>no book</p> :
    <div>
        <h4> Book {book.title}</h4>
        < hr />

        <div className= "row" >
            <div className="col-md-4" >
                <FormProvider {...methods}>
                    <form onSubmit={e => e.preventDefault} noValidate>
                        <input type="hidden" id="id" name="id" value={book.id} />
                        <input type="hidden" id="status" name="status" value={book.status}/>
                        <InputField label="Title" type="text" id="title" defaultVal={book.title} />
                        <InputField label="Author" type="text" id="author" defaultVal={book.author} />
                        <InputField label="Publisher" type="text" id="publisher" defaultVal={book.publisher} />
                        <InputField label="Publication Date" type="date" id="publicationDate" defaultVal={book.publicationDate} />
                        <InputField label="Price" type="number" id="pice" defaultVal={book.price} />

                        <div className="form-group" >
                            <button onClick={onSubmit} className="btn btn-primary">
                                Save
                            </button>
                        </div>
                        <div>
                            <a href="/catalogue" > Back to List </a>
                        </div>
                    </form>
                </FormProvider>
            </div>
        </div>
    </div>
    );

    async function getBook(bookId: number) {
        const response = await fetch('/book/' + bookId);
        //console.log(`\n************\n ${response.body} \n ***************8`)
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setBook(data);
        }
    }

};

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
                } />
        </div>
    );
};

export const BookEdit = () => {
    const { bookId } = useParams()
    const header = Header();
    const footer = Footer();
    return (
        <body>
            {header}
            <h1>Edit </h1>
            {EditBookForm(bookId!)}
            {footer}
        </body>
    )

}

export const BookCreate = () => {
    //const { bookId } = useParams()
    const header = Header();
    const footer = Footer();
    return (
        <body>
            {header}
            <h1>Create </h1>
            {EditBookForm("0", true)}
            {footer}
        </body>
    )
}
