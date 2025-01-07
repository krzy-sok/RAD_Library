import React, { useEffect, useState, useContext } from 'react';
import { Header, Footer } from '../shared/_Layout'

// Define the Book interface based on the model
export interface Book {
    id: number;
    title: string;
    author: string;
    publisher: string;
    publicationDate: string;
    price: number;
    status: string;
    hidden: boolean;
}

// Define the Props interface for the component
//interface BookTableProps {
//    books: Book[];
//    isAdmin: boolean; // Assuming you pass whether the user is an admin or not
//}

const BookTable = () => {
    const [books, setBooks] = useState<Book[]>();

    useEffect(() => {
        populateBookData();
    }, []);

    const contents = books === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started</em></p>
        :
        <table className="table">
            <thead>
                <tr>
                    <th>Title</th>
                    <th>Author</th>
                    <th>Publisher</th>
                    <th>Publication Date</th>
                    <th>Price</th>
                    <th>Status</th>
                    
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {books.map((book) => (
                    <tr key={book.id}>
                        <td>{book.title}</td>
                        <td>{book.author}</td>
                        <td>{book.publisher}</td>
                        <td>{book.publicationDate}</td>
                        <td>{book.price}</td>
                        <td>{book.status}</td>
                        <td>
                            <a href={`/detailsBook/${book.id}`}>Details</a> |
                            <a href={`/editBook/${book.id}`}>Edit</a> |
                            <a href={`/deleteBook/${book.id}`}>Delete</a>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>;


    const header = Header();
    const footer = Footer();

    return (
        <div>
            {header}
            <div>
                <h1>Books List</h1>
                <p></p>
                <a href="/createBook" >
                    <button className="btn btn-primary">
                        Create New book
                    </button>
                </a>
                {contents}
            </div>
            {footer}
        </div>
    );

    async function populateBookData() {
        const response = await fetch('books');
        console.log(response)
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setBooks(data);
        }
    }
};


export const Catalogue = () => {
    return BookTable()
}

//export default BookTable;
//{ isAdmin && <td>{book.hidden ? 'Yes' : 'No'}</td> }
//<td>
//    <a href={`/details/${book.id}`}>Details</a> |
//    {isAdmin && (
//        <>
            //<a href={`/edit/${book.id}`}>Edit</a> |
//            <a href={`/delete/${book.id}`}>Delete</a>
//        </>
//    )}
//</td>
//{ isAdmin && <th>Hidden</th> }