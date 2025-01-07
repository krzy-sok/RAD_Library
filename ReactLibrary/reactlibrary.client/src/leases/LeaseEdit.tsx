//import React, { useEffect, useState } from 'react';
//import { Header, Footer } from '../shared/_Layout'
//import { useParams } from 'react-router-dom';
//import { Book } from "./Catalogue";
//import { useForm, FormProvider } from "react-hook-form";
//import { InputField } from "../shared/InputField";
//import { Link, Navigate } from 'react-router-dom';
//import { useAuth } from '../shared/AuthProvider';    


//export const EditBookForm = (bookId:string, create=false) => {
//    const [book, setBook] = useState<Book>()
//    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)
//    useEffect(() => {
//        if (!create) {
//            getBook(parseInt(bookId!))
//        }
//        setBook({ title: "", author: "", publisher:"", publicationDate: Date(), price: 0 })

//    }, [bookId]);
//    const methods = useForm();
//    const onSubmit =  methods.handleSubmit(data => {
//        console.log(data)
//        if (!create) {
//            //call to api /book/bookID with put method
//            makePUTrequest(data);
//        }
//        else {
//            //call to api with post method
//            makePOSTrequest(data)
//        }
//    })
    
//    return (
//    book === undefined ? <p>no book</p> :
//    <div>
//        <h4> Book {book.title}</h4>
//        < hr />
//        { feedback}
//        <div className= "row" >
//            <div className="col-md-4" >
//                <FormProvider {...methods}>
//                    <form onSubmit={e => e.preventDefault()} noValidate>
//                        <input type="hidden" id="id" name="id" value={book.id} />
//                        <input type="hidden" id="status" name="status" value={book.status}/>
//                        <InputField label="Title" type="text" id="Title" defaultVal={book.title} />
//                        <InputField label="Author" type="text" id="Author" defaultVal={book.author} />
//                        <InputField label="Publisher" type="text" id="Publisher" defaultVal={book.publisher} />
//                        <InputField label="Publication Date" type="date" id="PublicationDate" defaultVal={book.publicationDate} />
//                        <InputField label="Price" type="number" id="Pice" defaultVal={book.price} />

//                        <div className="form-group" >
//                            <button onClick={onSubmit} className="btn btn-primary">
//                                Save
//                            </button>
//                        </div>
//                        <div>
//                            <Link to="/catalogue" > Back to List </Link>
//                        </div>
//                    </form>
//                </FormProvider>
//            </div>
//        </div>
//    </div>
//    );

//    async function getBook(bookId: number) {
//        const response = await fetch('/book/' + bookId);
//        //console.log(`\n************\n ${response.body} \n ***************8`)
//        if (response.ok) {
//            const data = await response.json();
//            //console.log(data)
//            setBook(data);
//        }
//    }

//    async function makePUTrequest(data) {
//        console.log(data)
//        //call to api /book/bookID with PUT method
//        console.log("\n*******\n\n PUT request \n\n *********\n")
//        const requestOptions = {
//            method: 'PUT',
//            headers: { 'Content-Type': 'application/json' },
//            body: JSON.stringify(data)
//        };
//        const response = await fetch('/book/' + bookId, requestOptions);
//        if (response.status == 404) {
//            setFeedback(<div style={{color: "red"}} >This book no longer exists</div>);
//        }
//        else if (response.status == 409) {
//            getBook(parseInt(bookId))
//            setFeedback(<div style={{ color: "red" }} >There was a concurrency event, please try again</div>);
//        }
//        else if (response.ok) {
//            getBook(parseInt(bookId))
//            setFeedback(<div style={{ color: "green" }} >Book edited succesfully</div>);
//        }
//    }

//    async function makePOSTrequest(data) {
//        console.log(data)
//        console.log("\n*******\n\n POST request \n\n *********\n")
//        //call to api /book/bookID with post method
//        const requestOptions = {
//            method: 'POST',
//            headers: { 'Content-Type': 'application/json' },
//            body: JSON.stringify(data)
//        };
//        const response = await fetch('/books', requestOptions);
//        if (response.status == 201) {
//            setFeedback(<div style={{ color: "green" }} >Book added succesfully</div>);
//        }
//        else {
//            setFeedback(<div style={{ color: "red" }} >Unexpected error while adding book</div>);
//        }
//    }

//};




//export const BookEdit = () => {
//    const { bookId } = useParams()
//    const header = Header();
//    const footer = Footer();
//    const { isadmin } = useAuth();
//    return ( isadmin?
//        <div>
//            {header}
//            <h1>Edit </h1>
//            {EditBookForm(bookId!)}
//            {footer}
//        </div>
//        : <Navigate to="/catalogue" />
//    )

//}

//export const BookCreate = () => {
//    //const { bookId } = useParams()
//    const header = Header();
//    const footer = Footer();
//    const { isadmin } = useAuth();
//    return (isadmin? 
//        <div>
//            {header}
//            <h1>Create </h1>
//            {EditBookForm("0", true)}
//            {footer}
//        </div>
//        : <Navigate to="/catalogue" />
//    )
//}
