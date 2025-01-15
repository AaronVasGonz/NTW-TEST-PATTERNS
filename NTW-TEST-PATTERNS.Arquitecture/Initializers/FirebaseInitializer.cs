using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;

namespace Arquitecture.Initializers;

public class FirebaseInitializer 
{

    private readonly string _fireBaseBucket;
    private FirebaseStorage _firebaseStorage;
    public FirebaseInitializer(string fireBaseBucket)
    {
        _fireBaseBucket = fireBaseBucket;
        Initialize();
    }

    private void Initialize()
    {
        _firebaseStorage = new FirebaseStorage(
            _fireBaseBucket,
            new FirebaseStorageOptions
            {
                ThrowOnCancel = true,
            });
    }

    public FirebaseStorage GetFirebaseStorage()
    {
        return _firebaseStorage;
    }
}