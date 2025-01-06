import React, { useEffect, useState } from 'react';
import { Header, Footer } from './shared/_Layout'
import { useParams } from 'react-router-dom';
import { Book } from "./Catalogue"

export const BookDetailsBlock = (bookId: string) => {
    const [book, setBook] = useState<Book>()

    useEffect(() => {
        getBook(parseInt(bookId!))
    }, [bookId]);

    console.log(book)
    //const details = book === undefined ?
    //    <p>no book</p>
    //    :
    return (
        book === undefined ? <p>no book</p> :
            <div>
                <h4>Book</h4>
                <hr />
                <dl className="row">
                    <dt className="col-sm-2">Title</dt>
                    <dd className="col-sm-10">{book.title}</dd>

                    <dt className="col-sm-2">Author</dt>
                    <dd className="col-sm-10">{book.author}</dd>

                    <dt className="col-sm-2">Publisher</dt>
                    <dd className="col-sm-10">{book.publisher}</dd>

                    <dt className="col-sm-2">Publication Date</dt>
                    <dd className="col-sm-10">{book.publicationDate}</dd>

                    <dt className="col-sm-2">Price</dt>
                    <dd className="col-sm-10">{book.price}</dd>

                    <dt className="col-sm-2">Status</dt>
                    <dd className="col-sm-10">{book.status}</dd>
                </dl>
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
}

export const BookDetails = () => {
    const { bookId } = useParams()
    //const detailsBlock = 
    const header = Header();
    const footer = Footer();
    return (
    <div>
        {header}
            <div>
                <h1>Details</h1>
                {BookDetailsBlock(bookId!)}
                <div>
                    {/*Conditional rendering based on user role and authentication */}
                    {/*{user?.isAuthenticated && !user?.roles.includes('Admin') && book?.id && (*/}
                    <button onClick={() => onReserve(book.id)} className="btn btn-primary">
                        Reserve
                    </button>
                    {/*)}*/}
                    <a href={`/editBook/${bookId}`}>Edit</a>
                    {' | '}
                    <a href={'/catalogue'}>Back to list</a>
                </div>
        </div>
        {footer}
    </div>
    )

    function onReserve(bookId: number) {
        console.log('reserving');
        return
    }
}

export const BookDelete = () => {
    const { bookId } = useParams()
    const [feedback, setFeedback] = useState<JSX.Element>(<div></div>)
    //const detailsBlock = 
    const header = Header();
    const footer = Footer();
    return (
        <div>
            {header}
            <div>
                <h1>Delete</h1>
                {feedback}
                {BookDetailsBlock(bookId!)}
                <div>
                    {/*Conditional rendering based on user role and authentication */}
                    {/*{user?.isAuthenticated && !user?.roles.includes('Admin') && book?.id && (*/}
                    <button onClick={() => DeleteConfirmed(parseInt(bookId!))} className="btn btn-danger">
                        Delete
                    </button>
                    {/*)}*/}
                    {' | '}
                    <a href={'/catalogue'}>Back to list</a>
                </div>
            </div>
            {footer}
        </div>
    );

    async function DeleteConfirmed(bookId: number) {
        console.log(`deleting book of id {bookID}`);
        //make api call to book/id as http delete
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
        };
        const response = await fetch('/book/' + bookId, requestOptions);
        if (response.status == 403) {
            setFeedback(<div style={{ color: "red" }} >Cannot delete leased book</div>);
        }
        else if (response.status == 202) {
            setFeedback(<div style={{ color: "green" }} >Book hidden</div>);
        }
        else if (response.ok) {
            setFeedback(<div style={{ color: "green" }} >Book deleted</div>);
        }
        else {
            setFeedback(<div style={{ color: "red" }} >Book does not exist or different error occured</div>);
        }
    }
}




